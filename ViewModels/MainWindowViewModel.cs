using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Ninject;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.Support;
using Theodorus2.Views;

namespace Theodorus2.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IDisposable
    {
        private readonly IQueryExecutionService _queryExecutor;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly TextDocument _document = new TextDocument();
        private readonly Subject<bool> _dirtyState = new Subject<bool>();
        private readonly ObservableAsPropertyHelper<bool> _dirtyPropertyHelper;
        private readonly ObservableAsPropertyHelper<bool> _hasResultsPropertyHelper; 

        private string _statusMessage = String.Empty;
        private bool _isWorking;
        private int _progress = -1;
        private bool _isProgressIndeterminate;
        private IEnumerable<IQueryResult> _queryResults;
        private string _selectedText;

        public MainWindowViewModel(IStatusListener listener, IQueryExecutionService queryExecutor)
        {
            _queryExecutor = queryExecutor;

            var exc = new ReactiveCommand(this.WhenAny(x => x.IsWorking, x => !x.Value));
            exc.Subscribe(x => Application.Current.Shutdown());
            ExitCommand = exc;
            _compositeDisposable.Add(exc);

            var about = new ReactiveCommand();
            about.Subscribe(x => AboutDialogService.ShowAboutDialog());
            AboutCommand = about;
            _compositeDisposable.Add(about);

            _compositeDisposable.Add(
                listener.Status.Subscribe(x =>
                {
                    if (x == null) return;
                    if (x.CurrentStatus.HasValue)
                    {
                        IsWorking = x.CurrentStatus.Value == ExecutionStatus.RunningIndeterminate ||
                                    x.CurrentStatus.Value == ExecutionStatus.RunningMonitored;
                        IsProgressIndeterminate = x.CurrentStatus.Value == ExecutionStatus.RunningIndeterminate;
                    }

                    if (x.Progress.HasValue)
                    {
                        Progress = x.Progress.Value;
                    }

                    if (x.Message != null)
                    {
                        StatusMessage = x.Message;
                    }

                }));

            var textChanged = Observable.FromEventPattern<EventHandler, EventArgs>(
                h => _document.TextChanged += h,
                h => _document.TextChanged -= h);

            _compositeDisposable.Add(
                textChanged.Subscribe(x => _dirtyState.OnNext(true)));

            _dirtyPropertyHelper = _dirtyState.DistinctUntilChanged().ToProperty(this, x => x.IsDirty);
            _compositeDisposable.Add(_dirtyPropertyHelper);

            var textExists = textChanged
                .Select(_ => _document.TextLength > 0)
                .DistinctUntilChanged()
                .StartWith(false);

            var execute = new ReactiveCommand(textExists);
            execute.RegisterAsyncTask(async _ =>
            {
                var toExecute = !String.IsNullOrEmpty(SelectedText) ? SelectedText : Document.Text;
                QueryResults = await queryExecutor.Execute(toExecute);
            });
            ExecuteCommand = execute;
            _compositeDisposable.Add(execute);

            _hasResultsPropertyHelper = this.WhenAny(x => x.QueryResults, x => x.Value != null && x.Value.Any())
                .DistinctUntilChanged()
                .ToProperty(this, x => x.HasResults);
            _compositeDisposable.Add(_hasResultsPropertyHelper);

            var open = new ReactiveCommand();
            _compositeDisposable.Add(
                open.Subscribe(x =>
                {
                    _queryExecutor.ConnectionString = ConnectionInformationService.GetConnectionString();
                    this.RaisePropertyChanged(r => r.IsConnected);
                }));
            OpenDatabaseCommand = open;
            _compositeDisposable.Add(open);

            var options = new ReactiveCommand();
            _compositeDisposable.Add(
                options.Subscribe(x => OptionsDialogService.ShowOptionsDialog()));
            _compositeDisposable.Add(options);
            OptionsCommand = options;

            var openQuery = new ReactiveCommand(this.WhenAny(x => x.IsConnected, x => x.Value));
            openQuery.RegisterAsyncTask(async x =>
            {
                if (IsDirty)
                {
                    if (!UserPromptingService.PromptUserYesNo("Open",
                        "Current document has not been saved, do you want to continue and loose those changes?"))
                    {
                        return;
                    }
                }

                var fileName = FileSelectionService.PromptToOpenFile("*.sql",
                    "SQL Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All Files (*.*)|*.*");

                if (fileName == null) return;

                string data = null;
                try
                {
                    data = await Task.Run(() => TextFileIOService.ReadFile(fileName));
                }
                catch (FileNotFoundException)
                {
                    UserPromptingService.DisplayAlert(String.Format("The file {0} was not found.", fileName));
                    return;
                }
                catch (IOException)
                {
                    UserPromptingService.DisplayAlert(String.Format("The file {0} was not read.", fileName));
                }

                Document.Text = data;
                _dirtyState.OnNext(false);

            });
            OpenQueryCommand = openQuery;
            _compositeDisposable.Add(openQuery);

            var saveQuery = new ReactiveCommand(this.WhenAny(x => x.IsDirty, x => x.Value));
            saveQuery.RegisterAsyncTask(async x =>
            {
                var fileName = FileSelectionService.PromptToSaveFile("*.sql",
                    "SQL Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All Files (*.*)|*.*");
                if (fileName != null)
                {
                    await Task.Run(() => TextFileIOService.WriteFile(fileName, Document.Text));
                }
                _dirtyState.OnNext(false);

            });
            _compositeDisposable.Add(saveQuery);
            SaveQueryCommand = saveQuery;

            var saveResults = new ReactiveCommand(Observable.Return(false));
            _compositeDisposable.Add(
                saveResults.Subscribe(x =>
                {
                    // not sure yet what to do here, have to wok out the execute stuff first.
                    // and how to display said results.
                }));
            SaveResultsCommand = saveResults;
            _compositeDisposable.Add(saveResults);
            
            StatusMessage = "Ready";
        }

        public ICommand ExecuteCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand OpenDatabaseCommand { get; private set; }
        public ICommand OpenQueryCommand { get; private set; }
        public ICommand SaveQueryCommand { get; private set; }
        public ICommand SaveResultsCommand { get; private set; }
        public ICommand OptionsCommand { get; private set; }
        
        public IAboutDialogService AboutDialogService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IAboutDialogService>();
            }
        }

        public IConnectionInformationService ConnectionInformationService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IConnectionInformationService>();
            }
        }

        public IOptionsDialogService OptionsDialogService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IOptionsDialogService>();
            }
        }

        public ITextFileIOService TextFileIOService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<ITextFileIOService>();
            }
        }

        public IFileSelectionService FileSelectionService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IFileSelectionService>();
            }
        }

        public IUserPromptingService UserPromptingService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IUserPromptingService>();
            }
        }

        public IEnumerable<IQueryResult> QueryResults
        {
            get { return _queryResults; }
            set { this.RaiseAndSetIfChanged(ref _queryResults, value); }
        }

        public string SelectedText
        {
            get { return _selectedText; }
            set { this.RaiseAndSetIfChanged(ref _selectedText, value); }
        }

        public bool HasResults
        {
            get { return _hasResultsPropertyHelper.Value; }
        }

        public bool IsDirty
        {
            get { return _dirtyPropertyHelper.Value; }
        }

        public bool IsConnected
        {
            get { return !String.IsNullOrWhiteSpace(_queryExecutor.ConnectionString); }            
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            private set { this.RaiseAndSetIfChanged(ref _statusMessage, value); }
        }

        public bool IsWorking
        {
            get { return _isWorking; }
            private set { this.RaiseAndSetIfChanged(ref _isWorking, value); }
        }

        public int Progress
        {
            get { return _progress; }
            private set { this.RaiseAndSetIfChanged(ref _progress, value); }
        }

        public bool IsProgressIndeterminate
        {
            get { return _isProgressIndeterminate; }
            private set { this.RaiseAndSetIfChanged(ref _isProgressIndeterminate, value); }
        }

        public TextDocument Document
        {
            get { return _document; }
        }

    
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
