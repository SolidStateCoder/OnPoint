using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using ReactiveUI;
using Splat;
using Splat.NLog;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;

namespace OnPoint.WpfTestApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var logDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OnPointTestApp"));
            if (!logDir.Exists)
            {
                logDir.Create();
                logDir.Refresh();
            }

            string logDirPath = Path.Combine(logDir.FullName, "Logs");
            LoggingConfiguration config = new LoggingConfiguration();

            FileTarget fileTarget = new FileTarget() { KeepFileOpen = false };
            string logFilePath = Path.Combine(logDirPath, $"{Environment.UserName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt");
            fileTarget.FileName = logFilePath;
            fileTarget.Layout = @"${date:format=hh\:mm\:ss.fff tt} ~ ${level} ~ ${message} ${exception:maxInnerExceptionLevel=3:format=ToString,StackTrace}";
            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, fileTarget));
            config.AddTarget("file", new AsyncTargetWrapper(fileTarget));
            LogManager.Configuration = config;

            NLog.ILogger logger = LogManager.GetCurrentClassLogger();
            logger.Fatal($"OnPoint Test App started.");

            Locator.CurrentMutable.Register(() => new CustomPropertyResolver(), typeof(ICreatesObservableForProperty));
            Locator.CurrentMutable.UseNLogWithWrappingFullLogger();

            new MainWindow() { DataContext = new RootVM() }.Show();
        }
    }


    // Solution for suppressing the warning: The class OnPoint.WpfTestApp.RootView property DescriptionText is a POCO type and won't send change notifications, WhenAny will only return a single value! 
    // https://stackoverflow.com/questions/30352447/using-reactiveuis-bindto-to-update-a-xaml-property-generates-a-warning
    public class CustomPropertyResolver : ICreatesObservableForProperty
    {
        public int GetAffinityForObject(Type type, string propertyName, bool beforeChanged = false)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(type))
                return 0;
            var fi = type.GetTypeInfo().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
              .FirstOrDefault(x => x.Name == propertyName);

            return fi != null ? 2 /* POCO affinity+1 */ : 0;
        }

        public IObservable<IObservedChange<object, object>> GetNotificationForProperty(object sender, System.Linq.Expressions.Expression expression, string propertyName, bool beforeChanged = false, bool suppressWarnings = false)
        {
            var fe = (FrameworkElement)sender;
            return Observable.Return(new ObservedChange<object, object>(sender, expression), new DispatcherScheduler(fe.Dispatcher))
                .Concat(Observable.Never<IObservedChange<object, object>>());
        }
    }
}