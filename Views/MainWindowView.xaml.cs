using System.Windows;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView : IAboutDialogService
    {
        private readonly MainWindowViewModel _vm;

        public MainWindowView(MainWindowViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }

        public void ShowAboutDialog()
        {
            MessageBox.Show(this, "About");
        }
    }
}
