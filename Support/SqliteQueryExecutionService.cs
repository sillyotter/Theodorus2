using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ninject.Infrastructure.Language;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    class SqliteQueryExecutionService : IQueryExecutionService
    {
        private static readonly Regex Cleaner = new Regex(@"(--.*$|/\*[^*]*?\*/)", RegexOptions.Compiled|RegexOptions.Multiline);
        private static readonly Regex Splitter = new Regex(@"^\s*GO;*\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public Task<IEnumerable<IQueryResult>> Execute(string query)
        {
            return Task.Run(() =>
            {
                var cleaned = Cleaner.Replace(query, "");
                var splitQuery = Splitter.Split(cleaned);
                
                var toBeExecuted =
                    from chunk in splitQuery
                    let c = chunk.Trim()
                    where c.Length != 0
                    select c;

                var results = new List<IQueryResult>();

                using (var con = new SQLiteConnection(ConnectionString))
                {
                    foreach (var chunk in toBeExecuted)
                    {
                        try
                        {
                            using (var cmd = con.CreateCommand())
                            {
                                var sw = Stopwatch.StartNew();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = chunk;
                                var ds = new DataSet();
                                var da = new SQLiteDataAdapter(cmd);
                                da.Fill(ds);
                                sw.Stop();
                                results.Add(new QueryResult(chunk, sw.ElapsedMilliseconds, ds));
                            }
                        }
                        catch (SQLiteException e)
                        {
                            results.Add(new QueryResult(chunk, 0, e));
                        }
                    }
                }
                return results.ToEnumerable();

            });
        }

        public string ConnectionString { get; set; }
    }
}
