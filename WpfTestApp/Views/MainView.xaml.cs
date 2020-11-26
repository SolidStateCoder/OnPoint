using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class MainView : ReactiveUserControl<MainVM>
    {
        public MainView()
        {
            Loaded += UserControl_Loaded;
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.LoadNumbersVM, v => v.LoadNumbersButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.LoadLettersVM, v => v.LoadLettersButton.DataContext)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (MainVM)DataContext;
    }
}