using System.Reactive.Disposables;
using System;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class ConnectionDialog : IDisposable
	{
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly ConnectionDialogViewModel _vm;
	    
		public ConnectionDialog(ConnectionDialogViewModel vm)
		{
		    _vm = vm;
		    InitializeComponent();
		    DataContext = _vm;

		    _compositeDisposable.Add(
		        _vm.Results.Subscribe(x =>
		        {
		            DialogResult = x;
		            Close();
		        }));
            _compositeDisposable.Add(_vm);
		}
        
        public string ConnectionString
        {
            get
            {
                return _vm.ConnectionString;
            }
            set
            {
                _vm.ConnectionString = value;
            }
        }

	    public void Dispose()
	    {
	        _compositeDisposable.Dispose();
	    }
	}
}
