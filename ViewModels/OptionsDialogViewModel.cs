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
        private int _resultLimit;
        private int _fontSize;
        private string _font;
        private bool _showLineNumbers;


        public OptionsDialogViewModel(ISettingsStorageService srv)
        {
            _srv = srv;

            _showLineNumbers = _srv.GetValue<bool>("ShowLineNumbers");
            _resultLimit = _srv.GetValue<int>("ResultLimit");
            _fontSize = _srv.GetValue<int>("FontSize");
            _font = _srv.GetValue<string>("Font");

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

       
        public int ResultsLimit
        {
            get { return _resultLimit; }
            set { this.RaiseAndSetIfChanged(ref _resultLimit, value); }
        }

        
        public int FontSize
        {
            get { return _fontSize; }
            set { this.RaiseAndSetIfChanged(ref _fontSize, value); }
        }

        public string Font
        {
            get { return _font; }
            set { this.RaiseAndSetIfChanged(ref _font, value); }
        }

        public bool ShowLineNumbers
        {
            get { return _showLineNumbers; }
            set { this.RaiseAndSetIfChanged(ref _showLineNumbers, value); }
        }

        private void Save()
        {
            _srv.SetValue("Font", _font);
            _srv.SetValue("ResultLimit", _resultLimit);
            _srv.SetValue("FontSize",_fontSize);
            _srv.SetValue("ShowLineNumbers", _showLineNumbers);
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
      
    }
}
