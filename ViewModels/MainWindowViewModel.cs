using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
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

        public MainWindowViewModel(IStatusListener listener)
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

            StatusMessage = "Ready";

        }

        public ICommand ExitCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        [Inject]
        public IAboutDialogService AboutDialogService { get; set; }


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


        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
