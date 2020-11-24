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
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public class LettersVM : MultiContentVM<Letter>
    {
        public CommandVM RefreshCmdVM { get => _RefreshCmdVM; set => this.RaiseAndSetIfChanged(ref _RefreshCmdVM, value); }
        private CommandVM _RefreshCmdVM = default;

        public bool IsRefreshing { get => _IsRefreshing?.Value ?? false; }
        readonly ObservableAsPropertyHelper<bool> _IsRefreshing = default;

        private ReactiveCommand<Unit, bool> RefreshCmd { get; set; }

        public LettersVM()
        {
            Title = "Letters";
            RefreshCmd = ReactiveCommand.CreateFromObservable(() =>
                Observable.FromAsync(ct => LoadLettersAsync(ct))
                    .TakeUntil(CancelCmd));

            RefreshCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(LoadLettersComplete);

            RefreshCmd.IsExecuting.ToProperty(this, x => x.IsRefreshing, out _IsRefreshing);

            RefreshCmdVM = new CommandVM(RefreshCmd, 60, 24, "Refresh", null, "Click this to load Letters");
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            // Simulate activation delay.
            await Task.Delay(1000);
            LoadLettersStart();
            return await base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(RefreshCmd.IsExecuting);
        protected override IList<IObservable<bool>> GetCancellableCommads() => base.GetCancellableCommads().AddAll(RefreshCmd.IsExecuting);

        private void LoadLettersStart() => RefreshCmd.Execute().Subscribe();

        private async Task<bool> LoadLettersAsync(CancellationToken token)
        {
            Application.Current.Dispatcher.Invoke(() => Contents.Clear());
            for (char x = 'A'; x <= 'Z'; x++)
            {
                // Using Dispatcher to demonstrate to the user that the operation is taking too long.
                Application.Current.Dispatcher.Invoke(() => Contents.Add(new Letter(x)));
                // Simulate network latency, allow user to cancel
                await Task.Delay(250, token);
            }
            return true;
        }

        private void LoadLettersComplete(bool result)
        {
            if (result)
            {
                Contents.Add(new Letter('-'));
            }
        }

        protected override string GetBusyOverrideMessage()
        {
            string retVal = base.GetBusyOverrideMessage();
            if (IsRefreshing) retVal = "Refreshing...";
            return retVal;
        }
    }
}