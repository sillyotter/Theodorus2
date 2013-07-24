using System;
using System.Windows.Input;
using Theodorus2.Support;

namespace Theodorus2.ViewModels
{
    // Use dapper to load a option set from a sqlite db stoed in user directory somewhere.
    // or, use the regular settings in teh app.config  or te registery
    // in any event, that  is up to the optionsstore service interface implementations
    public class OptionsDialogViewModel : ValidatableReactiveObject<OptionsDialogViewModel>
    {
        public bool ShowQueryWithResults
        {
            get { throw new NotImplementedException(); }
        }

        public int ResultsLimit
        {
            get { throw new NotImplementedException(); }
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
      
    }
}
