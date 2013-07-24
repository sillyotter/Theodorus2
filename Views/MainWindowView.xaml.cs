using System;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Support;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView : IAboutDialogService, IConnectionInformationService, IFileSelectorService, IOptionsDialogService, IDisposable
    {
        private readonly MainWindowViewModel _vm;

        public MainWindowView(MainWindowViewModel vm)
        {
            _vm = vm;
            DataContext = vm;
            InitializeComponent();

            using (var s = GetType().Assembly.GetManifestResourceStream("Theodorus2.Assets.SQL.xhsd"))
            {
                if (s == null) return;
                using (var reader = new XmlTextReader(s))
                {
                    TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
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
                ValidateNames = true,
            };

            return ofd.ShowDialog() == true ? ofd.FileName : null;
        }

        public void ShowAboutDialog()
        {
            var about = SharedContext.Instance.Kernel.Get<About>();
            about.Owner = this;
            about.ShowDialog();
        }

        public string GetConnectionString()
        {
            using (var con = SharedContext.Instance.Kernel.Get<ConnectionDialog>())
            {
                con.Owner = this;
                var res = con.ShowDialog();
                return res == true ? con.ConnectionString : null;
            }
        }

        public void ShowOptionsDialog()
        {
            var options = SharedContext.Instance.Kernel.Get<OptionsDialog>();
            options.Owner = this;
            options.ShowDialog();
        }

        public void Dispose()
        {
            _vm.Dispose();
        }
    }
}
