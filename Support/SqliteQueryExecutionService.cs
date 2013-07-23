using System.Threading.Tasks;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    class SqliteQueryExecutionService : IQueryExecutionService
    {
        public Task Execute(string query)
        {
            return Task.FromResult(true);
        }
    }
}
