using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name = "Skew")]
	public class Skew : SQLiteFunction
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
		    var vrnc = newData.Item3;
		    var skewness = newData.Item4;
		    var variance = vrnc/(n - 1);
		    var standardDeviation = Math.Sqrt(variance);

            return (double)n / ((n - 1) * (n - 2)) * (skewness / (variance * standardDeviation));
                  
		}
	}
}