using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name="Kurtosis")]
	public class Kurtosis : SQLiteFunction
	{
	    public override void Step(object[] args, int stepNumber, ref object contextData)
	    {
            VarianceHelper.SnKStep(args, stepNumber, ref contextData);
	    }
        
		public override object Final(object contextData)
		{
			var newData = contextData as Tuple<int, double, double, double, double>;
			if (newData == null) return null;

            var n = newData.Item1;
            var variance = newData.Item3;
            var kurtosis = newData.Item5;
            
            return ((double)n * n - 1) / ((n - 2) * (n - 3)) * (n * kurtosis / (variance * variance) - 3 + 6.0 / (n + 1));
		}
	}
 }