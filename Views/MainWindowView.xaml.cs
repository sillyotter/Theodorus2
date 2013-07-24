using System;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Ninject;
using Theodorus2.Interfaces;
using Theodorus2.Support;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView : IAboutDialogService, IConnectionInformationService, IDisposable
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

        public void ShowAboutDialog()
        {
            var about = SharedContext.Instance.Kernel.Get<About>();
            about.ShowDialog();
        }

        public string GetConnectionString()
        {
            using (var con = SharedContext.Instance.Kernel.Get<ConnectionDialog>())
            {
                var res = con.ShowDialog();
                return res == true ? con.ConnectionString : null;
            }
        }

        public void Dispose()
        {
            _vm.Dispose();
        }
    }
}
