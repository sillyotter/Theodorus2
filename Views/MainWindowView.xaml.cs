using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView
    {
        private readonly MainWindowViewModel _vm;

        public MainWindowView(MainWindowViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }
    }
}
