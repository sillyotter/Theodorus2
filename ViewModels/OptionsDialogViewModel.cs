using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
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
            AddValidator(x => ResultsLimit, () => ResultsLimit <= 0 ? "Result limit must be greater than 0" : null);

            var okCmd = new ReactiveCommand(HasErrorsObservable.Select(x => !x));
            _compositeDisposable.Add(
                okCmd.Subscribe(x =>
                {
                    _result.OnNext(true);
                    _result.OnCompleted();
                }));
            _compositeDisposable.Add(okCmd);
            OkCommand = okCmd;

            var cancelCommand = new ReactiveCommand();
            _compositeDisposable.Add(
                okCmd.Subscribe(x =>
                {
                    _result.OnNext(false);
                    _result.OnCompleted();
                }));
            _compositeDisposable.Add(cancelCommand);
            CancelCommand = cancelCommand;
        }
        
        public IObservable<bool> Results
        {
            get { return _result; }
        }

        public int ResultsLimit
        {
            get { return Settings.Default.ResultLimit; }
            set
            {
                Settings.Default.ResultLimit = value;
                Settings.Default.Save();
            }
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
      
    }
}
