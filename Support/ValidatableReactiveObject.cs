using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace Theodorus2.Support
{
    public abstract class ValidatableReactiveObject<TSource> : ReactiveObject, INotifyDataErrorInfo, IDisposable where TSource: class
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private readonly ConcurrentDictionary<string, List<Func<string>>> _validators =
            new ConcurrentDictionary<string, List<Func<string>>>();

        private readonly ConcurrentDictionary<string, List<string>> _errors =
            new ConcurrentDictionary<string, List<string>>();

       

        protected void AddValidator<TValue>(Expression<Func<TSource, TValue>> selector, Func<string> validator)
        {
            var props = ReactiveUiExtensions.GetPropName(selector);

            _validators.AddOrUpdate(
                props,
                s => new List<Func<string>> { validator },
                (s, list) => new List<Func<string>>(list) { validator });

            RunValidators(props);
        }

        private void RunValidators(string propName)
        {
            var errors = _validators.GetOrAdd(propName, new List<Func<string>>()).Select(v => v());
            _errors[propName] = errors.Where(i => i != null).ToList();
            this.RaisePropertyChanged(x => x.HasErrors);
        }

        protected ValidatableReactiveObject()
        {
            var obs = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => PropertyChanged += handler, handler => PropertyChanged -= handler)
                .Select(@event => @event.EventArgs.PropertyName)
                .Where(x => x != "HasErrors")
                .Do(RunValidators)
                .Publish()
                .RefCount();

            _compositeDisposable.Add(
              obs.Subscribe(propName => OnErrorsChanged(new DataErrorsChangedEventArgs(propName))));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> result;
            if (_errors.TryGetValue(propertyName, out result))
            {
                return result;
            }
            return new string[0];
        }

        public bool HasErrors
        {
            get { return _errors.Any(x => x.Value.Any()); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public virtual void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}