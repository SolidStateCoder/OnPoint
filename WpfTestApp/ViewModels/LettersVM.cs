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
        public CommandVM RefreshVM{ get => _RefreshVM; set => this.RaiseAndSetIfChanged(ref _RefreshVM, value); }
        private CommandVM _RefreshVM= default;

        public bool IsRefreshing { get => _IsRefreshing?.Value ?? false; }
        readonly ObservableAsPropertyHelper<bool> _IsRefreshing = default;

        private ReactiveCommand<Unit, bool> Refresh { get; set; }

        public LettersVM()
        {
            Title = "Letters";
            Refresh = ReactiveCommand.CreateFromObservable(() =>
                Observable.FromAsync(ct => LoadLettersAsync(ct))
                    .TakeUntil(Cancel));

            Refresh
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(LoadLettersComplete);

            Refresh.IsExecuting.ToProperty(this, x => x.IsRefreshing, out _IsRefreshing);

            RefreshVM= new CommandVM(Refresh, 60, 24, "Refresh", null, "Click this to load Letters");
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            // Simulate activation delay.
            await Task.Delay(1000);
            LoadLettersStart();
            return await base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(Refresh.IsExecuting);
        protected override IList<IObservable<bool>> GetCancellableCommads() => base.GetCancellableCommads().AddAll(Refresh.IsExecuting);

        private void LoadLettersStart() => Refresh.Execute().Subscribe();

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