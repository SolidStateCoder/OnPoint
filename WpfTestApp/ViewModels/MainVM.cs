using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class MainVM : DualContentVM<IViewModel, IViewModel>
    {
        public CommandVM LoadNumbersVM{ get => _LoadNumbersVM; set => this.RaiseAndSetIfChanged(ref _LoadNumbersVM, value); }
        private CommandVM _LoadNumbersVM= default;

        public CommandVM LoadLettersVM{ get => _LoadLettersVM; set => this.RaiseAndSetIfChanged(ref _LoadLettersVM, value); }
        private CommandVM _LoadLettersVM= default;

        public PeopleVM PeopleVM { get => _PeopleVM; set => this.RaiseAndSetIfChanged(ref _PeopleVM, value); }
        private PeopleVM _PeopleVM = default;

        public MainVM()
        {
            LoadNumbersVM= new CommandVM(ReactiveCommand.Create(LoadNumbers, WhenContentNull_NotBusy), 
                90, 24, "Load Numbers", null, "Click this to load Numbers");

            LoadLettersVM= new CommandVM(ReactiveCommand.Create(LoadLetters, WhenContent2Null_NotBusy), 
                90, 24, "Load Letters", null, "Click this to load Letters");

            PeopleVM = new PeopleVM(new PersonService());
        }

        private void LoadNumbers() => Content = new NumbersVM();

        private void LoadLetters() => Content2 = new LettersVM();
    }
}