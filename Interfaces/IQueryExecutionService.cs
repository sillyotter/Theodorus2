using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theodorus2.Interfaces
{
    public interface IQueryExecutionService
    {
        Task<IEnumerable<IQueryResult>> Execute(string query);
        string ConnectionString { get; set; }
    }
}