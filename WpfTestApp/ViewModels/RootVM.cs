using OnPoint.ViewModels;
using ReactiveUI;

namespace OnPoint.WpfTestApp
{
    public class RootVM : ApplicationRootVM
    {
        public string Description { get => _Description; set => this.RaiseAndSetIfChanged(ref _Description, value); }
        private string _Description = default;

        public RootVM()
        {
            Title = "Wpf Test App for OnPoint";
            Description = "On Point - Enhances the Reactive UI MVVM framework with easy to use classes for the most common use cases.";
            HUDMessage = "Welcome to the On Point Wpf Demo app!";

            // These position values could be restored from saved preferences to place the window in in last known location.
            AppWidth = 800;
            AppHeight = 600;
            AppMinWidth = 400;
            AppMinHeight = 300;
            AppTop = 50;
            AppLeft = 50;

            Content = new MainVM();
        }
    }
}