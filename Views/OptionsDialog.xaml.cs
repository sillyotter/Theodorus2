using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class OptionsDialog
    {
        private readonly OptionsDialogViewModel _vm;

        public OptionsDialog(OptionsDialogViewModel vm)
        {
            _vm = vm;
            InitializeComponent();
            DataContext = _vm;
        }
    }
}
