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
        public CommandVM RefreshCmdVM { get => _RefreshCmdVM; set => this.RaiseAndSetIfChanged(ref _RefreshCmdVM, value); }
        private CommandVM _RefreshCmdVM = default;

        private ReactiveCommand<Unit, List<Number>> RefreshCmd { get; set; }

        public NumbersVM()
        {
            Title = "Numbers";
            RefreshCmd = ReactiveCommand.CreateFromTask(LoadNumbersAsync, WhenNotBusy);
            RefreshCmd
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(LoadNumbersComplete);

            RefreshCmdVM = new CommandVM(RefreshCmd, 60, 24, "Refresh", null, "Click this to load different Numbers");
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            // Simulate activation delay.
            await Task.Delay(3000);
            LoadNumbersStart();
            return await base.Activated(disposable);
        }

        protected override IList<IObservable<bool>> GetBusyCommands() => base.GetBusyCommands().AddAll(RefreshCmd.IsExecuting);

        private void LoadNumbersStart() => RefreshCmd.Execute().Subscribe();

        private async Task<List<Number>> LoadNumbersAsync()
        {
            BusyMessageOverride = "Refreshing...";
            // Simulate network latency.
            await Task.Delay(3000);
            List<Number> retVal = new List<Number>();
            int target = new Random().Next(10000);
            int start = new Random().Next(target);
            for(int x = start; x < target; x++)
            {
                retVal.Add(new Number(x));
            }
            return retVal;
        }

        private void LoadNumbersComplete(List<Number> numbers) => Contents.ClearAndAddRange(numbers);
    }
}