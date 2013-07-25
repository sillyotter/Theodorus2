﻿using System;
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
        private readonly IStatusReporter _reporter;
        private static readonly Regex Cleaner = new Regex(@"(--.*$|/\*[^*]*?\*/)", RegexOptions.Compiled|RegexOptions.Multiline);
        private static readonly Regex Splitter = new Regex(@"^\s*GO;*\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public SqliteQueryExecutionService(IStatusReporter reporter)
        {
            _reporter = reporter;
        }

        public Task<IEnumerable<IQueryResult>> Execute(string query)
        {
            if (String.IsNullOrEmpty(ConnectionString)) throw new InvalidOperationException("No connection string");

            return Task.Run(() =>
            {
                using (_reporter.BeginMonitoredWork("Executing..."))
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
                        var chunkedList = toBeExecuted.ToList();
                        var progressChunk = 100/chunkedList.Count;

                        foreach (var chunk in chunkedList)
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
                            _reporter.ReportProgress(progressChunk);
                        }
                    }
                    return results.ToEnumerable();
                }
            });
        }

        public string ConnectionString { get; set; }
    }
}
