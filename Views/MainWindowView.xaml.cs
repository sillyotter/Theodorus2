using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
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
    public partial class MainWindowView : IAboutDialogService, IConnectionInformationService, 
        IFileSelectionService, IOptionsDialogService, IUserPromptingService, IResultsPresenter,
        IDisposable
    {
        private readonly MainWindowViewModel _vm;
        private readonly IResultRenderer _renderer;

        public MainWindowView(MainWindowViewModel vm, IResultRenderer renderer)
        {
            _vm = vm;
            _renderer = renderer;
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
            //I tried to do this with an attached property, but it didn't work right...
            TextEditor.TextArea.SelectionChanged += (sender, args) => _vm.SelectedText = TextEditor.SelectedText;
        }

        public string PromptToOpenFile(string defaultExtension, string filters)
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

        public string PromptToSaveFile(string defaultExtension, string filters)
        {
            var ofd = new SaveFileDialog()
            {
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = true,
                OverwritePrompt = true,
                DefaultExt = defaultExtension,
                Filter = filters,
                FilterIndex = 0,
                RestoreDirectory = true,
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
            using (var options = SharedContext.Instance.Kernel.Get<OptionsDialog>())
            {
                options.Owner = this;
                options.ShowDialog();
            }
        }

        public void Dispose()
        {
            _vm.Dispose();
        }

        public bool PromptUserYesNo(string title, string question)
        {
            return MessageBox.Show(this, question, title, MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No) == MessageBoxResult.Yes;
        }

        public void DisplayAlert(string message)
        {
            MessageBox.Show(this, message, "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                MessageBoxResult.OK);
        }

        public async Task PresentResults(IEnumerable<IQueryResult> results)
        {
            Browser.NavigateToString(await _renderer.RenderResults(results));
        }
        
        private void MainWindowView_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = _vm.IsWorking;
        }
    }
}
