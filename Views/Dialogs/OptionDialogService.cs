using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Support;

namespace Theodorus2.Views.Dialogs
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