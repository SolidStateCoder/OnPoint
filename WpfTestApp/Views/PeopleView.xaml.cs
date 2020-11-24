using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace OnPoint.WpfTestApp
{
    public partial class PeopleView : ReactiveUserControl<PeopleVM>
    {
        public PeopleView()
        {
            Loaded += UserControl_Loaded;
            InitializeComponent();
#if DEBUG
            DebugOutputView debugOutputView = new DebugOutputView();
            DockPanel.SetDock(debugOutputView, Dock.Right);
            MainDock.Children.Insert(0, debugOutputView);
#endif
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.BusyIndicator.Visibility)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.MainDock.IsEnabled, x => !x)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.BusyMessage, v => v.BusyText.Text)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SelectedContent.FirstName, v => v.FirstNameText.Text)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SelectedContent.LastName, v => v.LastNameText.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.CancelBoundCmd, v => v.CancelButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsCancelEnabled, v => v.CancelButton.Visibility)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.RefreshItemsCmdVM, v => v.RefreshButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.AddNewItemCmdVM, v => v.AddButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.DeleteItemCmdVM, v => v.DeleteButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SaveChangedItemsCmdVM, v => v.SaveButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SearchItemsCmdVM, v => v.SearchButton.DataContext)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SearchTerm, v=> v.SearchTermText.Text)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (PeopleVM)DataContext;
    }
}