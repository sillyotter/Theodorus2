using System;
using System.Net.Mime;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Ninject;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.Support;

namespace Theodorus2.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IDisposable
    {
        private readonly IQueryExecutionService _queryExecutor;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private string _statusMessage = String.Empty;
        private bool _isWorking;
        private int _progress = -1;
        private bool _isProgressIndeterminate;
        private readonly TextDocument _document = new TextDocument();

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

            var textExists = Observable.FromEventPattern<EventHandler, EventArgs>(
                h => _document.TextChanged += h,
                h => _document.TextChanged -= h)
                .Select(_ => _document.TextLength > 0)
                .StartWith(false);

            var execute = new ReactiveCommand(textExists);
            execute.RegisterAsyncTask(async _ => await queryExecutor.Execute(_document.Text));
            ExecuteCommand = execute;
            _compositeDisposable.Add(execute);

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
        public ICommand GotoCommand { get; private set; }
        
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
