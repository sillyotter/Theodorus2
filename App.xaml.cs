using System.Windows;
using Ninject;
using Theodorus2.Properties;
using Theodorus2.Support;
using Theodorus2.Views;

namespace Theodorus2
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Settings.Default.Upgrade();
            var mainWindow = SharedContext.Instance.Kernel.Get<MainWindowView>();
            mainWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
