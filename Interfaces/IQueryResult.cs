using System;
using System.Data;

namespace Theodorus2.Interfaces
{
    public interface IQueryResult
    {
        string Query { get; }
        long Duration { get; }
        DataSet Results { get; }
        Exception Exception { get; }
    }
}