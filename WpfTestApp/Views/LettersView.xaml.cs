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
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel = (LettersVM)DataContext;
    }
}