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
    public class SqliteQueryExecutionService : IQueryExecutionService
    {
        private readonly IStatusReporter _reporter;
        private readonly ISettingsStorageService _settings;
        private static readonly Regex Cleaner = new Regex(@"(--.*$|/\*[^*]*?\*/)", RegexOptions.Compiled|RegexOptions.Multiline);
        private static readonly Regex Splitter = new Regex(@"^\s*GO;*\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public SqliteQueryExecutionService(IStatusReporter reporter, ISettingsStorageService settings)
        {
            _reporter = reporter;
            _settings = settings;
        }

        public Task<IEnumerable<IQueryResult>> Execute(string query)
        {
            if (String.IsNullOrEmpty(ConnectionString)) throw new InvalidOperationException("No connection string");
            var resultLimit = _settings.GetValue<int>("ResultLimit");
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
                        con.Open();
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

                                    // this is annoying, dataadapter.fill() has overrides that work on limiting the number of records
                                    // but most of them don't work right.  the closet i got was ignoring the limits on multiple tables.
                                    using (var r = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                                    {
                                        do
                                        {
                                            var schtab = r.GetSchemaTable();
                                            if (schtab == null) throw new InvalidOperationException();
                                            
                                            var colNames = new List<string>();
                                            foreach (var item in schtab.Rows.Cast<DataRow>())
                                            {
                                                var colName = (string) item["ColumnName"];
                                                while (colNames.Contains(colName))
                                                {
                                                    colName += "1";
                                                }
                                                item["ColumnName"] = colName;
                                                colNames.Add(colName);
                                            }

                                            var ndt = new DataTable();
                                            ndt.Columns.AddRange(schtab.Rows.Cast<DataRow>().Select(x => new DataColumn((string)x["ColumnName"],(Type)x["DataType"])).ToArray());
                                            var row = new object[schtab.Rows.Count];
                                            var count = 0;
                                            while (r.Read() && count < resultLimit)
                                            {
                                                r.GetValues(row);
                                                ndt.LoadDataRow(row, LoadOption.OverwriteChanges);
                                                count ++;
                                            }
                                            ds.Tables.Add(ndt);

                                        } while (r.NextResult());
                                    }
                                    
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
