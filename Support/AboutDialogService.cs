using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Views;

namespace Theodorus2.Support
{
    public class AboutDialogService : IAboutDialogService
    {
        public void ShowAboutDialog()
        {
            var about = SharedContext.Instance.Kernel.Get<About>();
            about.Owner = Application.Current.MainWindow;
            about.ShowDialog();
        }
    }
}