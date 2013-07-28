using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theodorus2.Interfaces
{
    public interface IResultRenderer
    {
        Task<string> RenderResults(IEnumerable<IQueryResult> resultSets);
    }
}