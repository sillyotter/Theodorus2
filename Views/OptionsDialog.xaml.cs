using System;
using System.Reactive.Disposables;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class OptionsDialog : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public OptionsDialog(OptionsDialogViewModel vm)
        {
            
            InitializeComponent();
            DataContext = vm;

            _compositeDisposable.Add(
                vm.Results.Subscribe(x =>
                {
                    DialogResult = x;
                    Close();
                }));
            _compositeDisposable.Add(vm);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
