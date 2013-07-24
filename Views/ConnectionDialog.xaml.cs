using System.Reactive.Disposables;
using Microsoft.Win32;
using Theodorus2.Interfaces;
using System;

namespace Theodorus2.Views
{
    interface IFileSelectorService
    {
        string PromptForFile(string defaultExtension, string filters);
    }

	public partial class ConnectionDialog : IFileSelectorService, IDisposable
	{
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly IConnectionDialogViewModel _vm;
	    
		public ConnectionDialog(IConnectionDialogViewModel vm)
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

	    public string PromptForFile(string defaultExtension, string filters)
	    {
            var ofd = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = defaultExtension,
                Filter = filters,
                FilterIndex = 0,
                RestoreDirectory = true,
                Multiselect = false,
                DereferenceLinks = true,
                ValidateNames = true
            };

	        return ofd.ShowDialog() == true ? ofd.FileName : null;
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
