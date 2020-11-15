using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class NumbersView : ReactiveUserControl<NumbersVM>
    {
        public NumbersView()
        {
            Loaded += UserControl_Loaded;
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.BusyIndicator.Visibility)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.MainDock.IsEnabled, x => !x)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.BusyMessage, v => v.BusyText.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.RefreshCmdVM, v => v.RefreshButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SelectedContent, v => v.SelectedLetterText.Text, x => $"You selected: {x}")
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SelectedContent, v => v.SelectedLetterText.Visibility, x => x == null ? Visibility.Collapsed : Visibility.Visible)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (NumbersVM)DataContext;
    }
}