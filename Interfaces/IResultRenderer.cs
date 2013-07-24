using System.Collections.Generic;

namespace Theodorus2.Interfaces
{
    public interface IResultRenderer
    {
        string RenderResults(IEnumerable<IQueryResult> input);
    }
}