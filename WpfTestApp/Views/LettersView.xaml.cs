using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class LettersView : ReactiveUserControl<LettersVM>
    {
        public LettersView()
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

                this.BindCommand(ViewModel, vm => vm.CancelBoundCmd, v => v.CancelButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsCancelEnabled, v => v.CancelButton.Visibility)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (LettersVM)DataContext;
    }
}