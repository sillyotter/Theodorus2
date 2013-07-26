using System;
using System.Data;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class QueryResult : IQueryResult
    {
        public QueryResult(string query, long duration, DataSet results)
        {
            Query = query;
            Duration = duration;
            Results = results;
        }

        public QueryResult(string query, long duration, Exception exception)
        {
            Query = query;
            Duration = duration;
            Exception = exception;
        }
        
        public string Query { get; private set; }
        public long Duration { get; private set; }
        public DataSet Results { get; private set; }
        public Exception Exception { get; private set; }
    }
}