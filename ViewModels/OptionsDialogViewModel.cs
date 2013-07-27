using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Microsoft.Win32;
using ReactiveUI;
using Theodorus2.Properties;
using Theodorus2.Support;

namespace Theodorus2.ViewModels
{
    // Use dapper to load a option set from a sqlite db stoed in user directory somewhere.
    // or, use the regular settings in teh app.config  or te registery
    // in any event, that  is up to the optionsstore service interface implementations
    public class OptionsDialogViewModel : ValidatableReactiveObject<OptionsDialogViewModel>, IDisposable
    {
        private readonly Subject<bool> _result = new Subject<bool>();
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        public OptionsDialogViewModel()
        {
            _resultLimit = Settings.Default.ResultLimit;

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
            Settings.Default.ResultLimit = _resultLimit;
            Settings.Default.Save();
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
      
    }
}
