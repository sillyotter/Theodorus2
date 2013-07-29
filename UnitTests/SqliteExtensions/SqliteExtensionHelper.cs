using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace UnitTests.SqliteExtensions
{
    internal static class SqliteExtensionHelper
    {
        public static TRet AggregateFunction<TSource,TRet>(Func<SQLiteFunction> ctor, IEnumerable<TSource> data)
        {
            object contextData = null;
            var target = ctor();

            var enumerable = data as TSource[] ?? data.ToArray();
            foreach (var element in enumerable.Zip(Enumerable.Range(0, enumerable.Count()), (i, i1) => new { Data = i, Index = i1 }))
            {
                target.Step(new object[]{element.Data}, element.Index, ref contextData);
            }

            return (TRet) target.Final(contextData);
        }

        public static TRet AggregateFunction<TSource, TRet>(Func<SQLiteFunction> ctor, IEnumerable<IEnumerable<TSource>> data)
        {
            object contextData = null;
            var target = ctor();

            var enumerable = data as IEnumerable<TSource>[] ?? data.ToArray();
            var start = new List<List<TSource>>(Enumerable.Repeat(new List<TSource>(), enumerable.First().Count())).AsEnumerable();
            var res = enumerable.Aggregate(start, (current, item) => current.Zip(item, (a, b) => new List<TSource>(a) {b})).Select((d,i) => new {Data = d, Index = i});
            
            foreach (var element in res)
            {
                target.Step(element.Data.Cast<object>().ToArray(), element.Index, ref contextData);
            }

            return (TRet)target.Final(contextData);
        }

        public static TRet ScalarFunction<TSource, TRet>(Func<SQLiteFunction> ctor, IEnumerable<TSource> data)
        {
            var target = ctor();
            return (TRet)target.Invoke(data.Cast<object>().ToArray());
        }
    }
}