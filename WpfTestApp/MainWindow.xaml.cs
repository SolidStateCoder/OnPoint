using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class MainWindow : ReactiveWindow<RootVM>
    {
        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.Title)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppHeight, v => v.Height)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppWidth, v => v.Width)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppMinHeight, v => v.MinHeight)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppMinWidth, v => v.MinWidth)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppTop, v => v.Top)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.AppLeft, v => v.Left)
                    .DisposeWith(disposable);
            });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) => ViewModel = (RootVM)DataContext;
    }
}