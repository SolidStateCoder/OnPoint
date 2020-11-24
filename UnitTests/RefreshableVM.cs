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
        public ReactiveCommand<Unit, string> RefreshCmd { get; set; }

        public bool IsRefreshing { get => _IsRefreshing?.Value ?? false; }
        readonly ObservableAsPropertyHelper<bool> _IsRefreshing = default;

        public RefreshableVM()
        {
            CanActivatedBeRequested = true;

            RefreshCmd = ReactiveCommand.CreateFromObservable(() =>
                CreateAsyncObservable(RefreshAsync, true)
                   , WhenNotBusy);

            RefreshCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshComplete);

            RefreshCmd.IsExecuting.ToProperty(this, x => x.IsRefreshing, out _IsRefreshing);

            Content = (_Counter++).ToString();
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            return base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(RefreshCmd.IsExecuting);
        protected override IList<IObservable<bool>> GetCancellableCommads() => base.GetCancellableCommads().AddAll(RefreshCmd.IsExecuting);
        protected override IList<IObservable<Exception>> GetAllCommandThrownExceptions() => base.GetAllCommandThrownExceptions().AddAll(RefreshCmd.ThrownExceptions);

        private async Task<string> RefreshAsync(CancellationToken token)
        {
            await Task.Delay(2000, token);
            return (_Counter++).ToString();
        }

        private void RefreshComplete(string result) => Content = result;

        internal void StartRefresh() => RefreshCmd.Execute().Subscribe();
    }
}