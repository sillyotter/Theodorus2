using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView : IAboutDialogService
    {
        public MainWindowView(MainWindowViewModel vm)
        {
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
            MessageBox.Show(this, "About");
        }
    }
}
