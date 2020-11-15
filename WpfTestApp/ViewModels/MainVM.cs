using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OnPoint.WpfTestApp
{
    public class MainVM : DualContentVM<IViewModel, IViewModel>
    {
        public CommandVM LoadNumbersCmdVM { get => _LoadNumbersCmdVM; set => this.RaiseAndSetIfChanged(ref _LoadNumbersCmdVM, value); }
        private CommandVM _LoadNumbersCmdVM = default;

        public CommandVM LoadLettersCmdVM { get => _LoadLettersCmdVM; set => this.RaiseAndSetIfChanged(ref _LoadLettersCmdVM, value); }
        private CommandVM _LoadLettersCmdVM = default;

        private ReactiveCommand<Unit, Unit> LoadNumbersCmd { get; set; }
        private ReactiveCommand<Unit, Unit> LoadLettersCmd { get; set; }

        public MainVM()
        {
            LoadNumbersCmd = ReactiveCommand.Create(LoadNumbers, WhenContentNull_NotBusy);

            LoadNumbersCmdVM = new CommandVM(LoadNumbersCmd, 90, 24, "Load Numbers", null, "Click this to load Numbers");

            LoadLettersCmd = ReactiveCommand.Create(LoadLetters, WhenContent2Null_NotBusy);

            LoadLettersCmdVM = new CommandVM(LoadLettersCmd, 90, 24, "Load Letters", null, "Click this to load Letters");
        }

        protected async override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            return await base.Activated(disposable);
        }

        private void LoadNumbers() => Content = new NumbersVM();

        private void LoadLetters()
        {
            int x = 0;
        }
    }
}