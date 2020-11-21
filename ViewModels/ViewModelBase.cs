using Autofac;
using NLog;
using OnPoint.Universal;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// The base class for all view models in OnPoint.
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject, IViewModel, IActivatableViewModel, IRoutableViewModel, IEquatable<ViewModelBase>
    {
        public uint ViewModelTypeId { get => _ViewModelTypeId; set => this.RaiseAndSetIfChanged(ref _ViewModelTypeId, value); }
        private uint _ViewModelTypeId = default;

        public bool IsEnabled { get => _IsEnabled; set => this.RaiseAndSetIfChanged(ref _IsEnabled, value); }
        private bool _IsEnabled = true;

        public bool IsVisible { get => _IsVisible; set => this.RaiseAndSetIfChanged(ref _IsVisible, value); }
        private bool _IsVisible = true;

        public bool IsBusy { get => _IsBusy; private set => this.RaiseAndSetIfChanged(ref _IsBusy, value); }
        private bool _IsBusy = false;

        public bool IsCancelEnabled { get => _IsCancelEnabled; set => this.RaiseAndSetIfChanged(ref _IsCancelEnabled, value); }
        private bool _IsCancelEnabled = false;

        public string BusyMessage { get => _BusyMessage; set => this.RaiseAndSetIfChanged(ref _BusyMessage, value); }
        private string _BusyMessage = "Loading...";

        public string BusyMessageOverride { get => _BusyMessageOverride; set => this.RaiseAndSetIfChanged(ref _BusyMessageOverride, value); }
        private string _BusyMessageOverride = default;

        public string HUDMessage { get => _HUDMessage; set => this.RaiseAndSetIfChanged(ref _HUDMessage, value); }
        private string _HUDMessage = default;

        public string Title { get => _Title; set => this.RaiseAndSetIfChanged(ref _Title, value); }
        private string _Title = default;

        public bool IsActivated { get => _IsActivated; set => this.RaiseAndSetIfChanged(ref _IsActivated, value); }
        private bool _IsActivated = false;

        public bool IsSelected { get => _IsSelected; set => this.RaiseAndSetIfChanged(ref _IsSelected, value); }
        private bool _IsSelected = false;

        public bool IsShowingErrorMessage { get { return _IsShowingErrorMessage.Value; } }
        private ObservableAsPropertyHelper<bool> _IsShowingErrorMessage = default;

        public bool IsShowingHUDMessage { get { return _IsShowingHUDMessage.Value; } }
        private ObservableAsPropertyHelper<bool> _IsShowingHUDMessage = default;

        public string ErrorMessage { get => _ErrorMessage; set => this.RaiseAndSetIfChanged(ref _ErrorMessage, value); }
        private string _ErrorMessage = default;

        public string UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        // CancelCmd can always be executed. CancelBoundCmd can only be executed when a cancellable command is executing.
        protected ReactiveCommand<Unit, Unit> CancelCmd { get; private set; }

        /// <summary>
        /// Can be used by the UI to call <see cref="CancelCmd"/>. CancelCmd can always be executed, but CancelBoundCmd can only be 
        /// executed when a cancellable command is executing which makes it ideal for binding to the UI.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelBoundCmd { get => _CancelBounCmd; private set => this.RaiseAndSetIfChanged(ref _CancelBounCmd, value); }
        private ReactiveCommand<Unit, Unit> _CancelBounCmd = default;

        public ReactiveCommand<Unit, bool> CloseErrorMessageCmd { get; protected set; }
        public ReactiveCommand<Unit, string> CloseHUDMessageCmd { get; protected set; }

        private static long _IDIndex = 0;
        public long ViewModelId { get; }

        protected IObservable<bool> WhenNotBusy { get; }
        protected IObservable<bool> AlwaysOff { get; }
        protected Action<bool> _ToggleBusy { get; }
        protected Action<bool, string> _ToggleBusyMessage { get; }

        private int _BusyCount = 0;
        private object _BusyLock = new object();

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected bool IsEnabledWhenBusy { get; set; } = false;
        protected bool CanActivatedBeRequested { get; set; } = false;

        /// <summary>
        /// Used by Reactive UI to activate this view model when it enters the visual tree. It should be not touched.
        /// </summary>
        public ViewModelActivator Activator { get; }
        protected ILifetimeScope LifeTimeScope { get; private set; }

        // Reference counter so we know when we can dispose of a scope. Using "nested" or "owned" lifetime scopes is 
        // a solution to having one (or more) global containers as you see in most of the examples on the web. Global
        // containers are an anti-pattern. Reactive UI recommends using lifetime scopes and provides the facility to
        // manager them through their extensive use of the Dispose pattern.
        // https://reactiveui.net/docs/handbook/when-activated/
        private static readonly Dictionary<ILifetimeScope, int> _LifetimeScopeCounts = new Dictionary<ILifetimeScope, int>();

        // https://www.reactiveui.net/docs/handbook/routing/
        public ViewModelBase(ILifetimeScope lifeTimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default)
        {
            ViewModelId = Interlocked.Increment(ref _IDIndex);
            ViewModelTypeId = viewModelTypeId;
            Activator = new ViewModelActivator();
            LifeTimeScope = lifeTimeScope;
            if (LifeTimeScope != null)
            {
                lock (_LifetimeScopeCounts)
                {
                    if (_LifetimeScopeCounts.Count > 100000)
                    {
                        Logger.Fatal("There are over 100,000 active AutoFac lifetime scopes. This is probably a memory leak.");
                    }
                    //
                    if (!_LifetimeScopeCounts.ContainsKey(LifeTimeScope))
                    {
                        _LifetimeScopeCounts.Add(LifeTimeScope, 1);
                    }
                    else
                    {
                        _LifetimeScopeCounts[LifeTimeScope] = _LifetimeScopeCounts[LifeTimeScope] + 1;
                    }
                }
            }

            WhenNotBusy = this.WhenAny(vm => vm.IsBusy, x => x.Value == false);
            AlwaysOff = this.WhenAny(vm => vm.ViewModelId, x => x.Value == 0);

            _ToggleBusy = x => { if (x) StartBusy(); else StopBusy(); };
            _ToggleBusyMessage = (x, y) => { if (x) StartBusy(y); else StopBusy(); };

            CancelCmd = ReactiveCommand.Create(Cancel);
            CloseErrorMessageCmd = ReactiveCommand.Create(() => false);
            CloseHUDMessageCmd = ReactiveCommand.Create(() => HUDMessage = default);

            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            UrlPathSegment = urlPathSegment;

            try
            {
                // https://reactiveui.net/docs/handbook/when-activated/
                this.WhenActivated(async disposable =>
                {
                    StartBusy("Activating...");
                    await Activated(disposable);
                    StopBusy();

                    Disposable
                        .Create(() =>
                        {
                            if (LifeTimeScope != null)
                            {
                                lock (_LifetimeScopeCounts)
                                {
                                    if (_LifetimeScopeCounts.ContainsKey(LifeTimeScope))
                                    {
                                        _LifetimeScopeCounts[LifeTimeScope] = _LifetimeScopeCounts[LifeTimeScope] - 1;
                                        if (_LifetimeScopeCounts[LifeTimeScope] < 1)
                                        {
                                            Deactivated(LifeTimeScope);
                                            _LifetimeScopeCounts.Remove(LifeTimeScope);
                                        }
                                    }
                                    else
                                    {
                                        Deactivated(LifeTimeScope);
                                    }
                                }
                            }
                        })
                        .DisposeWith(disposable);

                    IsActivated = true;
                });
            }
            catch (FileNotFoundException fnfe) { Logger.Error(fnfe, "Xamarin Android bug https://github.com/reactiveui/ReactiveUI/issues/2049"); } // Xamarin Android bug https://github.com/reactiveui/ReactiveUI/issues/2049
        }

        protected void StartBusy(string message = default)
        {
            lock (_BusyLock)
            {
                IsEnabled = IsEnabledWhenBusy;
                _BusyCount++;
                IsBusy = true;

                if (message.IsNothing())
                {
                    if (BusyMessageOverride.IsNothing())
                    {
                        BusyMessage = "Loading...";
                    }
                    else
                    {
                        BusyMessage = BusyMessageOverride;
                        BusyMessageOverride = default;
                    }
                }
                else
                {
                    BusyMessage = message;
                }
            }
        }

        protected void StopBusy(bool forceStop = false)
        {
            lock (_BusyLock)
            {
                IsEnabled = true;
                if (forceStop)
                {
                    _BusyCount = 1;
                }

                _BusyCount--;

                if (_BusyCount <= 0)
                {
                    _BusyCount = 0;
                    IsBusy = false;
                    BusyMessage = default;
                }
            }
        }

        // Added to visual tree.
        protected virtual async Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            IList<IObservable<bool>> commandsIsExecuting = GetCancellableCommads();
            IsCancelEnabled = commandsIsExecuting.AnyNonNulls();
            CancelBoundCmd = ReactiveCommand.Create(() => { CancelCmd.Execute().Subscribe(); }, Observable.Merge(commandsIsExecuting));

            IList<IObservable<bool>> busyCmds = GetBusyCommands();
            Observable.Merge(GetBusyCommands())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Skip(busyCmds.Count)
                .Subscribe(x => _ToggleBusy(x));

            IList<IObservable<Exception>> throwers = GetAllCommandThrownExceptions();
            throwers.Add(CancelCmd.ThrownExceptions);
            throwers.Add(CancelBoundCmd.ThrownExceptions);
            throwers.Add(CloseErrorMessageCmd.ThrownExceptions);
            throwers.Add(CloseHUDMessageCmd.ThrownExceptions);

            foreach (var thrower in throwers)
            {
                thrower.Subscribe(x => { ErrorMessage = $"Error in {this}: {x.Message}"; });
            }

            List<IObservable<bool>> errorables = new List<IObservable<bool>>();
            foreach (IObservable<Exception> thrower in throwers)
            {
                errorables.Add(thrower.Select(x => true));
            }
            if (CancelCmd != null)
            {
                errorables.Add(CancelCmd.ThrownExceptions.Select(x => true));
            }
            errorables.Add(CloseErrorMessageCmd);

            _IsShowingErrorMessage = Observable.Merge(errorables.ToArray())
                .Delay(TimeSpan.FromMilliseconds(250), RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.IsShowingErrorMessage, false, true, RxApp.MainThreadScheduler);

            _IsShowingHUDMessage = this.WhenAnyValue(vm => vm.HUDMessage)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => x.IsSomething())
                .ToProperty(this, x => x.IsShowingHUDMessage, false, true, RxApp.MainThreadScheduler);

            return ExecutionResultMessage.Success;
        }

        // Removed from visual tree.
        protected virtual ExecutionResultMessage Deactivated(ILifetimeScope scope)
        {
            try
            {
                CancelCmd?.Execute()?.Subscribe();
                StopBusy();
                scope?.Dispose();
            }
            catch { }
            return ExecutionResultMessage.Success;
        }

        protected virtual IList<IObservable<bool>> GetCancellableCommads() => new List<IObservable<bool>>();
        protected virtual IList<IObservable<Exception>> GetAllCommandThrownExceptions() => new List<IObservable<Exception>>();
        protected virtual IList<IObservable<bool>> GetBusyCommands() => new List<IObservable<bool>>();

        protected void Cancel() => AfterCancel();

        protected virtual void AfterCancel() { }

        public override string ToString() => Title ?? base.ToString();

        public bool Equals(ViewModelBase other) => ViewModelId == other?.ViewModelId;

        /// <summary>
        /// If two or more view models share the same view, this method can be used to ensure they all get activated together.
        /// <see cref="CanActivatedBeRequested"/> must be set to True.
        /// </summary>
        /// <param name="disposable">The <see cref="CompositeDisposable"/> used to dispose bindings</param>
        public async void RequestActivated(CompositeDisposable disposable)
        {
            if (CanActivatedBeRequested)
            {
                await Activated(disposable);
                IsActivated = true;
            }
        }

        protected IObservable<T> CreateAsyncObservable<T>(Func<T> funk, bool canCancel = false)
        {
            IObservable<T> retVal = Observable.FromAsync(ct => Task.Run(funk, ct));
            if (canCancel)
            {
                retVal = retVal.TakeUntil(CancelCmd);
            }
            return retVal;
        }

        protected IObservable<T> CreateAsyncObservable<T>(Func<CancellationToken, Task<T>> funk, bool canCancel = false)
        {
            IObservable<T> retVal = Observable.FromAsync(funk);
            if (canCancel)
            {
                retVal = retVal.TakeUntil(CancelCmd);
            }
            return retVal;
        }
    }
}