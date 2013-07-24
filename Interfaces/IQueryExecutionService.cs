using System.Threading.Tasks;

namespace Theodorus2.Interfaces
{
    public interface IQueryExecutionService
    {
        Task Execute(string query);
        string ConnectionString { get; set; }
    }
}