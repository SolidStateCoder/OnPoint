using Autofac;
using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnPoint.UnitTests
{
    public class RefreshableVM : ContentVM<string>
    {
        private int _Counter = 0;
        public ReactiveCommand<Unit, string> Refresh { get; set; }

        public bool IsRefreshing { get => _IsRefreshing?.Value ?? false; }
        readonly ObservableAsPropertyHelper<bool> _IsRefreshing = default;

        public RefreshableVM(string content = null, ILifetimeScope lifetimeScope = null) : base(content, lifetimeScope)
        {
            CanActivatedBeRequested = true;

            Refresh = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshAsync, true)
                   , WhenNotBusy);

            Refresh
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshComplete);

            Refresh.IsExecuting.ToProperty(this, x => x.IsRefreshing, out _IsRefreshing);

            Content = (_Counter++).ToString();
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            return base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(Refresh.IsExecuting);
        protected override IList<IObservable<bool>> GetCancellableCommads() => base.GetCancellableCommads().AddAll(Refresh.IsExecuting);
        protected override IList<IObservable<Exception>> GetAllCommandThrownExceptions() => base.GetAllCommandThrownExceptions().AddAll(Refresh.ThrownExceptions);

        private async Task<string> RefreshAsync(CancellationToken token)
        {
            await Task.Delay(2000, token);
            return (_Counter++).ToString();
        }

        private void RefreshComplete(string result) => Content = result;

        internal void StartRefresh() => Refresh.Execute().Subscribe();
    }
}