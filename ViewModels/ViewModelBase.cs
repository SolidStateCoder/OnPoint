using Autofac;
using OnPoint.Universal;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public abstract class ViewModelBase : ReactiveObject, IViewModel, IActivatableViewModel, IRoutableViewModel, IEquatable<ViewModelBase>, IEnableLogger
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

        public string BusyMessageOverride { get => _BusyMessageOverride; private set => this.RaiseAndSetIfChanged(ref _BusyMessageOverride, value); }
        private string _BusyMessageOverride = default;

        public string HUDMessage { get => _HUDMessage; set => this.RaiseAndSetIfChanged(ref _HUDMessage, value); }
        private string _HUDMessage = default;

        public string Title { get => _Title; set => this.RaiseAndSetIfChanged(ref _Title, value); }
        private string _Title = default;

        public bool IsActivated { get => _IsActivated; set => this.RaiseAndSetIfChanged(ref _IsActivated, value); }
        private bool _IsActivated = false;

        public bool IsSelected { get => _IsSelected; set => this.RaiseAndSetIfChanged(ref _IsSelected, value); }
        private bool _IsSelected = false;

        public bool IsShowingErrorMessage { get { return _IsShowingErrorMessage?.Value ?? false; } }
        private ObservableAsPropertyHelper<bool> _IsShowingErrorMessage = default;

        public bool IsShowingHUDMessage { get { return _IsShowingHUDMessage?.Value ?? false; } }
        private ObservableAsPropertyHelper<bool> _IsShowingHUDMessage = default;

        public string ErrorMessage { get => _ErrorMessage; set => this.RaiseAndSetIfChanged(ref _ErrorMessage, value); }
        private string _ErrorMessage = default;

        public string UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        // Cancel can always be executed. CancelBound can only be executed when a cancellable command is executing.
        protected ReactiveCommand<Unit, Unit> Cancel { get; private set; }

        /// <summary>
        /// Can be used by the UI to call <see cref="Cancel"/>. Cancel can always be executed, but CancelBound can only be 
        /// executed when a cancellable command is executing which makes it ideal for binding to the UI.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelBound { get => _CancelBound; private set => this.RaiseAndSetIfChanged(ref _CancelBound, value); }
        private ReactiveCommand<Unit, Unit> _CancelBound = default;

        public ReactiveCommand<Unit, bool> CloseErrorMessage { get; protected set; }
        public ReactiveCommand<Unit, string> CloseHUDMessage { get; protected set; }

        private static long _IDIndex = 0;
        public long ViewModelId { get; }

        protected IObservable<bool> WhenNotBusy { get; }
        protected IObservable<bool> AlwaysOff { get; }
        protected Action<bool> _ToggleBusy { get; }
        protected Action<bool, string> _ToggleBusyMessage { get; }

        private int _BusyCount = 0;
        private object _BusyLock = new object();

        protected bool IsEnabledWhenBusy { get; set; } = false;
        protected bool CanActivatedBeRequested { get; set; } = false;

#if DEBUG
        private static string NL { get; } = Environment.NewLine;
        public virtual string DebugOutput =>
            $"Title: {Title}{NL}" +
            $"IsEnabled: {IsEnabled}{NL}" +
            $"IsVisible: {IsVisible}{NL}" +
            $"IsBusy: {IsBusy}{NL}" +
            $"IsActivated: {IsActivated}{NL}" +
            $"IsSelected: {IsSelected}{NL}" +
            $"IsShowingErrorMessage: {IsShowingErrorMessage}{NL}" +
            $"IsShowingHUDMessage: {IsShowingHUDMessage}{NL}" +
            $"IsCancelEnabled: {IsCancelEnabled}{NL}" +
            $"BusyMessage: {BusyMessage}{NL}" +
            $"HUDMessage: {HUDMessage}{NL}" +
            $"ErrorMessage: {ErrorMessage}{NL}";
#endif

        /// <summary>
        /// Used by Reactive UI to activate this view model when it enters the visual tree. It should be not touched.
        /// </summary>
        public ViewModelActivator Activator { get; }
        protected ILifetimeScope LifetimeScope { get; private set; }

        // Reference counter so we know when we can dispose of a scope. Using "nested" or "owned" lifetime scopes is 
        // a solution to having one (or more) global containers as you see in most of the examples on the web. Global
        // containers are an anti-pattern. Reactive UI recommends using lifetime scopes and provides the facility to
        // manager them through their extensive use of the Dispose pattern.
        // https://reactiveui.net/docs/handbook/when-activated/
        private static readonly Dictionary<ILifetimeScope, int> _LifetimeScopeCounts = new Dictionary<ILifetimeScope, int>();

        /// <summary>
        /// Can be used by unit tests or system code to ensure there are no memory leaks of lifetimescopes.
        /// </summary>
        public static int LifetimeScopeCounts => _LifetimeScopeCounts.Values.Sum();

        // https://www.reactiveui.net/docs/handbook/routing/
        public ViewModelBase(ILifetimeScope lifetimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default)
        {
            ViewModelId = Interlocked.Increment(ref _IDIndex);
            ViewModelTypeId = viewModelTypeId;
            Activator = new ViewModelActivator();
            LifetimeScope = lifetimeScope;
            if (LifetimeScope != null)
            {
                lock (_LifetimeScopeCounts)
                {
                    if (_LifetimeScopeCounts.Count > 100000)
                    {
                        this.Log().Fatal("There are over 100,000 active AutoFac lifetime scopes. This is probably a memory leak.");
                    }
                    //
                    if (!_LifetimeScopeCounts.ContainsKey(LifetimeScope))
                    {
                        _LifetimeScopeCounts.Add(LifetimeScope, 1);
                    }
                    else
                    {
                        _LifetimeScopeCounts[LifetimeScope] = _LifetimeScopeCounts[LifetimeScope] + 1;
                    }
                }
            }

            WhenNotBusy = this.WhenAny(vm => vm.IsBusy, x => x.Value == false);
            AlwaysOff = this.WhenAny(vm => vm.ViewModelId, x => x.Value == 0);

            _ToggleBusy = x => { if (x) StartBusy(); else StopBusy(); };
            _ToggleBusyMessage = (x, y) => { if (x) StartBusy(y); else StopBusy(); };

            Cancel = ReactiveCommand.Create(CancelRequested);
            CloseErrorMessage = ReactiveCommand.Create(() => false);
            CloseHUDMessage = ReactiveCommand.Create(() => HUDMessage = default);

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
#if DEBUG
                    Observable.Merge(
                        this.WhenAnyValue(x => x.Title).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsEnabled).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsVisible).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsBusy).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsActivated).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsSelected).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsShowingErrorMessage).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsShowingHUDMessage).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.IsCancelEnabled).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.BusyMessage).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.HUDMessage).Select(_ => Unit.Default),
                        this.WhenAnyValue(x => x.ErrorMessage).Select(_ => Unit.Default)
                        )
                        .Subscribe(_ => this.RaisePropertyChanged(nameof(DebugOutput)))
                        .DisposeWith(disposable);
#endif

                    IsActivated = true;
                });
            }
            catch (FileNotFoundException fnfe) { this.Log().Error(fnfe, "Xamarin Android bug https://github.com/reactiveui/ReactiveUI/issues/2049"); } // Xamarin Android bug https://github.com/reactiveui/ReactiveUI/issues/2049
        }

        protected void StartBusy(string message = default)
        {
            Debug($"StartBusy {message}");
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
            Debug($"StopBusy {forceStop}");
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

        // Triggered when the corresponding view has been added to the visual tree.
        protected virtual async Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            Debug($"Activated");
            Disposable
                .Create(() =>
                {
                    if (LifetimeScope != null)
                    {
                        lock (_LifetimeScopeCounts)
                        {
                            if (_LifetimeScopeCounts.ContainsKey(LifetimeScope))
                            {
                                _LifetimeScopeCounts[LifetimeScope] = _LifetimeScopeCounts[LifetimeScope] - 1;
                                if (_LifetimeScopeCounts[LifetimeScope] < 1)
                                {
                                    Deactivated(LifetimeScope);
                                    _LifetimeScopeCounts.Remove(LifetimeScope);
                                }
                            }
                            else
                            {
                                Deactivated(LifetimeScope);
                            }
                        }
                    }
                })
                .DisposeWith(disposable);

            IList<IObservable<bool>> commandsIsExecuting = GetCancellableCommads();
            IsCancelEnabled = commandsIsExecuting.AnyNonNulls();
            CancelBound = ReactiveCommand.Create(() => { ObservableExtensions.Subscribe(this.Cancel.Execute()); }, Observable.Merge(commandsIsExecuting));

            IList<IObservable<bool>> busyCmds = GetBusyCommands();
            Observable.Merge(GetBusyCommands())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Skip(busyCmds.Count)
                .Subscribe(x => _ToggleBusy(x));

            IList<IObservable<Exception>> throwers = GetAllCommandThrownExceptions();
            throwers.Add(Cancel.ThrownExceptions);
            throwers.Add(CancelBound.ThrownExceptions);
            throwers.Add(CloseErrorMessage.ThrownExceptions);
            throwers.Add(CloseHUDMessage.ThrownExceptions);

            foreach (var thrower in throwers)
            {
                thrower.Subscribe(x => { ErrorMessage = $"Error in {this}: {x.Message}"; });
            }

            List<IObservable<bool>> errorables = new List<IObservable<bool>>();
            foreach (IObservable<Exception> thrower in throwers)
            {
                errorables.Add(thrower.Select(x => true));
            }
            if (Cancel != null)
            {
                errorables.Add(Cancel.ThrownExceptions.Select(x => true));
            }
            errorables.Add(CloseErrorMessage);

            _IsShowingErrorMessage = Observable.Merge(errorables.ToArray())
                .Delay(TimeSpan.FromMilliseconds(250), RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.IsShowingErrorMessage, false, true, RxApp.MainThreadScheduler);

            _IsShowingHUDMessage = this.WhenAnyValue(vm => vm.HUDMessage)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => x.IsSomething())
                .ToProperty(this, x => x.IsShowingHUDMessage, false, true, RxApp.MainThreadScheduler);

            this.WhenAnyValue(vm => vm.IsBusy)
                .Subscribe(_ => { BusyMessageOverride = GetBusyOverrideMessage(); })
                .DisposeWith(disposable);

            return ExecutionResultMessage.Success;
        }

        // Removed from visual tree.
        protected virtual ExecutionResultMessage Deactivated(ILifetimeScope scope)
        {
            Debug($"Deactivated");
            try
            {
                Cancel?.Execute()?.Subscribe();
                StopBusy();
                scope?.Dispose();
            }
            catch(Exception ex)
            {
                this.Log().Error(ex, $"{ViewModelId} - {this}: Deactivated.");
            }
            return ExecutionResultMessage.Success;
        }

        protected virtual IList<IObservable<bool>> GetCancellableCommads() => new List<IObservable<bool>>();
        protected virtual IList<IObservable<Exception>> GetAllCommandThrownExceptions() => new List<IObservable<Exception>>();
        protected virtual IList<IObservable<bool>> GetBusyCommands() => new List<IObservable<bool>>();

        protected void CancelRequested() => AfterCancel();

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
            Debug($"RequestActivated");
            if (CanActivatedBeRequested)
            {
                if (LifetimeScope != null && (disposable == null || disposable.IsDisposed))
                {
                    throw new NotSupportedException($"In order to do manual view model activation with a LifetimeScope, CanActivatedBeRequested must be to true and a non-null and non-disposed CompositeDisposable must be provided.{Environment.NewLine}Manual view model activation should be avoided; see: https://reactiveui.net/docs/handbook/when-activated/");
                }
                else
                {
                    await Activated(disposable);
                    IsActivated = true;
                }
            }
            else
            {
                this.Log().Warn($"Activation requested on {this}, but CanActivatedBeRequested is set to false.");
            }
        }

        protected IObservable<T> CreateAsyncObservable<T>(Func<T> funk, bool canCancel = false)
        {
            IObservable<T> retVal = Observable.FromAsync(ct => Task.Run(funk, ct));
            if (canCancel)
            {
                retVal = retVal.TakeUntil(Cancel);
            }
            return retVal;
        }

        protected IObservable<T> CreateAsyncObservable<T>(Func<CancellationToken, Task<T>> funk, bool canCancel = false)
        {
            IObservable<T> retVal = Observable.FromAsync(funk);
            if (canCancel)
            {
                retVal = retVal.TakeUntil(Cancel);
            }
            return retVal;
        }

        protected virtual string GetBusyOverrideMessage() => null;

        protected void Debug(string message) => this.Log().Debug($"{ViewModelId} - {this}: {message}.");
    }
}