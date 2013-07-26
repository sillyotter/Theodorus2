using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;

namespace Theodorus2.Views
{
    public partial class MainWindowView : IDisposable
    {
        private readonly MainWindowViewModel _vm;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public MainWindowView(MainWindowViewModel vm, IResultRenderer renderer)
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
            // not right, instead of has results, i should really be watching a iobservable tat tells me when ever something is there
            _vm.WhenAny(x => x.QueryResults, x => x.Value).Where(x => x != null).Subscribe(
                async x => Browser.NavigateToString(await renderer.RenderResults(x)));

            _compositeDisposable.Add(vm);

            TextEditor.TextArea.SelectionChanged += (sender, args) => _vm.SelectedText = TextEditor.SelectedText;
        }

        public void Dispose()
        {
            _vm.Dispose();
        }

        private void MainWindowView_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = _vm.IsWorking;
        }
    }
}
