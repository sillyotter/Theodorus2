using System;
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
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private string _statusMessage = String.Empty;
        private bool _isWorking;
        private int _progress = -1;
        private bool _isProgressIndeterminate;
        private readonly TextDocument _document = new TextDocument();

        public MainWindowViewModel(IStatusListener listener, IQueryExecutionService queryExecutor)
        {
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
                .Select(_ => _document.TextLength > 0);

            var execute = new ReactiveCommand(textExists);
            execute.RegisterAsyncTask(async _ => await queryExecutor.Execute(_document.Text));
            ExecuteCommand = execute;
            _compositeDisposable.Add(execute);

            StatusMessage = "Ready";

        }

        public ICommand ExecuteCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        public IAboutDialogService AboutDialogService
        {
            get
            {
                return SharedContext.Instance.Kernel.Get<IAboutDialogService>();
            }
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
