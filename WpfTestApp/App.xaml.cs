using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) => new MainWindow() { DataContext = new RootVM() }.Show();
    }
}