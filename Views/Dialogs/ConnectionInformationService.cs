using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Support;

namespace Theodorus2.Views.Dialogs
{
    public class ConnectionInformationService : IConnectionInformationService
    {
        public string GetConnectionString()
        {
            using (var con = SharedContext.Instance.Kernel.Get<ConnectionDialog>())
            {
                con.Owner = Application.Current.MainWindow;
                var res = con.ShowDialog();
                return res == true ? con.ConnectionString : null;
            }
        }
    }
}