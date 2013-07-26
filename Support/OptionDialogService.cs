using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Views;

namespace Theodorus2.Support
{
    public class OptionDialogService : IOptionsDialogService
    {
        public void ShowOptionsDialog()
        {
            using (var options = SharedContext.Instance.Kernel.Get<OptionsDialog>())
            {
                options.Owner = Application.Current.MainWindow;
                options.ShowDialog();
            }
        }
    }
}