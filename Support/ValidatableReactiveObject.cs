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
    abstract class ValidatableReactiveObject<TSource> : ReactiveObject, INotifyDataErrorInfo, IDisposable 
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private readonly ConcurrentDictionary<string, List<Func<string>>> _validators = 
            new ConcurrentDictionary<string, List<Func<string>>>(); 

        private readonly ConcurrentDictionary<string,List<string>> _errors =
            new ConcurrentDictionary<string, List<string>>();

        private readonly ObservableAsPropertyHelper<bool> _hasErrorsObservableAsPropertyHelper;

        protected void AddValidator<TValue>(Expression<Func<TSource, TValue>> selector, Func<string> validator)
        {
            var props = Reflection.ExpressionToPropertyNames(selector).Single();

            _validators.AddOrUpdate(
                props,
                s => new List<Func<string>> {validator},
                (s, list) => new List<Func<string>>(list) {validator});

            RunValidators(props);
        }

        private void RunValidators(string propName)
        {
            var errors = _validators.GetOrAdd(propName, new List<Func<string>>()).Select(v => v());
            _errors[propName] = errors.Where(i => i != null).ToList();
        }

        protected ValidatableReactiveObject()
        {
            var obs = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => PropertyChanged += handler, handler => PropertyChanged -= handler)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(@event => @event.EventArgs.PropertyName)
                .Do(RunValidators)
                .Publish()
                .RefCount();

            _hasErrorsObservableAsPropertyHelper = obs.Select(_ => _errors.Any(x => x.Value.Any()))
                .ToProperty(this, x => x.HasErrors, false, RxApp.MainThreadScheduler);

            _compositeDisposable.Add(
                obs.ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(propName => OnErrorsChanged(new DataErrorsChangedEventArgs(propName))));

            _compositeDisposable.Add(_hasErrorsObservableAsPropertyHelper);
        }

        protected IObservable<bool> HasErrorsObservable
        {
            get
            {
                return _hasErrorsObservableAsPropertyHelper;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> result;
            if (_errors.TryGetValue(propertyName, out result))
            {
                return result;
            }
            return new String[0];
        }

        public bool HasErrors {
            get { return _errors.Any(x => x.Value.Any()); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}