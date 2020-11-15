using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

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
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (RootVM)DataContext;
    }
}