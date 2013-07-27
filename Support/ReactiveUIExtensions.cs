using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Theodorus2.Support
{
    public static class ReactiveUIExtensions
    {
        public static void RaisePropertyChanged<TSource, TValue>(this TSource @this,
            params Expression<Func<TSource, TValue>>[] selectors) where TSource: ReactiveObject
        {
            foreach (var pname in selectors.Select(Reflection.ExpressionToPropertyNames))
            {
                @this.RaisePropertyChanged(pname.Single());
            }
        }

        public static IObservable<TSource> WhenPropertyChanged<TSource, TValue>(this IObservable<IObservedChange<TSource, TValue>> @this,
            Expression<Func<TSource, TValue>> selector)
        {
            var pname = Reflection.ExpressionToPropertyNames(selector);
            return @this
                .Where(x => x.PropertyName == pname.Single())
                .Select(x => x.Sender);
        }

        public static ReactiveCommand CreateWithInitialCondition(IObservable<bool> canExecute, bool initialCondition,
           bool allowConcurrentExecution = false, IScheduler sched = null)
        {
            return new ReactiveCommand(canExecute.StartWith(initialCondition), allowConcurrentExecution, sched);
        }

        public static IObservable<Unit> ExecuteCompleted(this ReactiveCommand @this)
        {
            return @this.IsExecuting.SkipWhile(x => x == false)
                .Where(x => x == false)
                .Select(_ => Unit.Default)
                .Publish()
                .RefCount();
        }

        public static Task ExecuteAsync(this IReactiveCommand cmd, object parameter = null)
        {
            if (!cmd.CanExecute(parameter)) throw new InvalidOperationException("Can't Execute");

            var watcher = Task.Run(async () =>
            {
                await cmd.IsExecuting.FirstAsync(x => x == false);
            });
            cmd.Execute(null);

            return watcher;
        }
    }
}