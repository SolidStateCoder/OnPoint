using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class MainVM : DualContentVM<IViewModel, IViewModel>
    {
        public CommandVM LoadNumbersCmdVM { get => _LoadNumbersCmdVM; set => this.RaiseAndSetIfChanged(ref _LoadNumbersCmdVM, value); }
        private CommandVM _LoadNumbersCmdVM = default;

        public CommandVM LoadLettersCmdVM { get => _LoadLettersCmdVM; set => this.RaiseAndSetIfChanged(ref _LoadLettersCmdVM, value); }
        private CommandVM _LoadLettersCmdVM = default;

        public PeopleVM PeopleVM { get => _PeopleVM; set => this.RaiseAndSetIfChanged(ref _PeopleVM, value); }
        private PeopleVM _PeopleVM = default;

        public MainVM()
        {
            LoadNumbersCmdVM = new CommandVM(ReactiveCommand.Create(LoadNumbers, WhenContentNull_NotBusy), 
                90, 24, "Load Numbers", null, "Click this to load Numbers");

            LoadLettersCmdVM = new CommandVM(ReactiveCommand.Create(LoadLetters, WhenContent2Null_NotBusy), 
                90, 24, "Load Letters", null, "Click this to load Letters");

            PeopleVM = new PeopleVM(new PersonService());
        }

        private void LoadNumbers() => Content = new NumbersVM();

        private void LoadLetters() => Content2 = new LettersVM();
    }
}