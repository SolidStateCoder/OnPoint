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

                this.OneWayBind(ViewModel, vm => vm.SelectedContent, v => v.FirstNameText.IsEnabled, x => x != null)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SelectedContent.FirstName, v => v.FirstNameText.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SelectedContent, v => v.LastNameText.IsEnabled, x => x != null)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SelectedContent.LastName, v => v.LastNameText.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.CancelBound, v => v.CancelButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsCancelEnabled, v => v.CancelButton.Visibility)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.RefreshItemsVM, v => v.RefreshButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.CreateNewItemVM, v => v.AddButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.DeleteItemVM, v => v.DeleteButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SaveItemVM, v => v.SaveButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SaveChangedItemsVM, v => v.SaveAllButton.DataContext)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SearchItemsVM, v => v.SearchButton.DataContext)
                    .DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.SearchTerm, v => v.SearchTermText.Text)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.IsShowingSearchResults, v => v.SearchIndicator.Visibility)
                    .DisposeWith(disposable);
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (PeopleVM)DataContext;
    }
}