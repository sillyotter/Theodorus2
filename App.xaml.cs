using System;
using System.Windows;
using Ninject;
using NLog;
using NLog.Config;
using Theodorus2.Properties;
using Theodorus2.Support;
using Theodorus2.Views;

namespace Theodorus2
{
    public partial class App
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public App()
        {
            Settings.Default.Upgrade();

            AppDomain.CurrentDomain.UnhandledException +=
                (o, args) => Log.Error((Exception)args.ExceptionObject, "Unhandled domain exception");

#if DEBUG
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.Debug.config");
#endif
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {            
            var mainWindow = SharedContext.Instance.Kernel.Get<MainWindowView>();
            mainWindow.Show();
        }
    }
}
