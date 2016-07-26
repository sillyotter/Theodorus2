using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.Legacy.ReactiveCommand;

namespace Theodorus2.Support
{
    public static class EnumEx
    {
        public static Array GetDistinctValues(Type @enum)
        {
            return Enum.GetValues(@enum).Cast<object>().Distinct().ToArray();
        }
    }

    public static class ReactiveUiExtensions
    {
        public static string GetPropName<TSource,TValue>(Expression<Func<TSource, TValue>> exp)
        {
            var expression = (MemberExpression)exp.Body;
            return expression.Member.Name;
        }

        public static void RaisePropertyChanged<TSource, TValue>(this TSource @this,
            params Expression<Func<TSource, TValue>>[] selectors) where TSource: ReactiveObject
        {
            foreach (var pname in selectors.Select(GetPropName))
            {
                @this.RaisePropertyChanged(pname);
            }
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