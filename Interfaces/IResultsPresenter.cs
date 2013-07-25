using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theodorus2.Interfaces
{
    public interface IResultsPresenter
    {
        Task PresentResults(IEnumerable<IQueryResult> results);
    }
}