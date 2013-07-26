using System.Collections.Generic;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name = "Median")]
	public class Median : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			dynamic item = args[0];
			var storedData = (contextData as List<double> ?? new List<double>());
            storedData.Add(item);
            contextData = storedData;
		}

        // could be changed to use selection algorithm to be a bit faster.
        // https://en.wikipedia.org/wiki/Selection_algorithm
		public override object Final(object contextData)
		{
			if (contextData == null) return null;
            var storedData = (contextData as List<double> ?? new List<double>());
            storedData.Sort();
		    var len = storedData.Count;
		    if (len%2 == 0)
		    {
		        return (storedData[(len/2)-1] + storedData[(len/2)])/2.0;
		    }
		    return storedData[((len+1)/2)-1];
		}
	}
}