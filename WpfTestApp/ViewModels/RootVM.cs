using OnPoint.Universal;
using OnPoint.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace OnPoint.WpfTestApp
{
    public class RootVM : ApplicationRootVM
    {
        public string Description { get => _Description; set => this.RaiseAndSetIfChanged(ref _Description, value); }
        private string _Description = default;

        public RootVM()
        {
            Title = "Wpf Test App for OnPoint";
            Description = "On Point - Enhances the Reactive UI MVVM framework with easy to use patterns for the most common use cases.";

            // These position values could be restored from saved preferences to place the window in in last known location.
            AppWidth = 800;
            AppHeight = 600;
            AppMinWidth = 400;
            AppMinHeight = 300;

            AppTop = 50;
            AppLeft = 50;
            // Or: DisplayState = AppDisplayState.Maximized;

            Content = new MainVM();
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            var retVal = base.Activated(disposable);
            HUDMessage = "Welcome to the On Point Wpf Demo app!";
            return retVal;
        }
    }
}