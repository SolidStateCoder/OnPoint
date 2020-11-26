using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Splat;
using Splat.NLog;
using System;
using System.IO;
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

            Locator.CurrentMutable.UseNLogWithWrappingFullLogger();

            new MainWindow() { DataContext = new RootVM() }.Show();
        }
    }
}