using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.Support;

namespace Theodorus2.ViewModels
{
    public class OptionsDialogViewModel : ValidatableReactiveObject<OptionsDialogViewModel>
    {
        private readonly ISettingsStorageService _srv;
        private readonly Subject<bool> _result = new Subject<bool>();
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public OptionsDialogViewModel(ISettingsStorageService srv)
        {
            _srv = srv;
            _resultLimit = _srv.GetValue<int>("ResultLimit");

            AddValidator(x => x.ResultsLimit, () => _resultLimit <= 0 ? "Result limit must be greater than 0" : null);

            var okCmd = new ReactiveCommand(this.WhenAny(x => x.HasErrors, x => !x.GetValue()));
            _compositeDisposable.Add(okCmd);
            OkCommand = okCmd;

            var cancelCommand = new ReactiveCommand();
            _compositeDisposable.Add(cancelCommand);
            CancelCommand = cancelCommand;

            _compositeDisposable.Add(
                okCmd.Select(x => true).Merge(cancelCommand.Select(x => false)).Subscribe(x =>
                {
                    _result.OnNext(x);
                    _result.OnCompleted();
                }));
            _compositeDisposable.Add(
                okCmd.Subscribe(x => Save())
                );

        }
        
        public IObservable<bool> Results
        {
            get { return _result; }
        }

        private int _resultLimit;

        public int ResultsLimit
        {
            get { return _resultLimit; }
            set { this.RaiseAndSetIfChanged(ref _resultLimit, value); }
        }

        private void Save()
        {
            _srv.SetValue("ResultLimit", _resultLimit);
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
      
    }
}
