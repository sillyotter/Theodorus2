using System.Windows;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Views;

namespace Theodorus2.Support
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