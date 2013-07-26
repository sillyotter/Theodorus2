using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Support;

namespace Theodorus2.Views.Dialogs
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