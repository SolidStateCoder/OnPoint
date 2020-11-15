using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using OnPoint.Universal;

namespace OnPoint.WpfTestApp
{
    public partial class RootView : ReactiveUserControl<RootVM>
    {
        public RootView()
        {
            Loaded += UserControl_Loaded;
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.Description, v => v.DescriptionText.Text)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppWidth, v => v.WidthSlider.Value)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.AppWidth, v => v.WidthText.Text)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppMinWidth, v => v.WidthSlider.Minimum)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppHeight, v => v.HeightSlider.Value)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.AppHeight, v => v.HeightText.Text)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppMinHeight, v => v.HeightSlider.Minimum)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.CloseHUDMessageCmd, v => v.CloseHUDButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.HUDMessage, v => v.HUDMessageText.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.HUDMessage, v => v.HUDPanel.Visibility, x => x.IsNothing() ? Visibility.Collapsed : Visibility.Visible)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (RootVM)DataContext;
    }
}