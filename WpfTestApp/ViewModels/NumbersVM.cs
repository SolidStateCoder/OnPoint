using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.WpfTestApp
{
    public class NumbersVM : MultiContentVM<Number>
    {
        public CommandVM RefreshVM{ get => _RefreshVM; set => this.RaiseAndSetIfChanged(ref _RefreshVM, value); }
        private CommandVM _RefreshVM= default;

        public bool IsRefreshing { get => _IsRefreshing?.Value ?? false; }
        readonly ObservableAsPropertyHelper<bool> _IsRefreshing = default;

        private ReactiveCommand<Unit, List<Number>> Refresh { get; set; }

        public NumbersVM()
        {
            Title = "Numbers";
            Refresh = ReactiveCommand.CreateFromTask(LoadNumbersAsync, WhenNotBusy);
            Refresh
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(LoadNumbersComplete);

            Refresh.IsExecuting.ToProperty(this, x => x.IsRefreshing, out _IsRefreshing);

            RefreshVM= new CommandVM(Refresh, 60, 24, "Refresh", null, "Click this to load different Numbers");
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            // Simulate activation delay.
            await Task.Delay(3000);
            LoadNumbersStart();
            return await base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(Refresh.IsExecuting);

        private void LoadNumbersStart() => Refresh.Execute().Subscribe();

        private async Task<List<Number>> LoadNumbersAsync()
        {
            // Simulate network latency.
            await Task.Delay(3000);
            List<Number> retVal = new List<Number>();
            int target = new Random().Next(10000);
            int start = new Random().Next(target);
            for (int x = start; x < target; x++)
            {
                retVal.Add(new Number(x));
            }
            return retVal;
        }

        private void LoadNumbersComplete(List<Number> numbers) => Contents.ClearAndAddRange(numbers);

        protected override string GetBusyOverrideMessage()
        {
            string retVal = base.GetBusyOverrideMessage();
            if (IsRefreshing) retVal = "Refreshing...";
            return retVal;
        }
    }
}