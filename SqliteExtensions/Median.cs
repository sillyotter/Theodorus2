using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name = "Median")]
	public class Median : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			dynamic item = args[0];
			var median = (contextData is double ? (double) contextData : 0);

			median += 0.001*Math.Sign(item - median);

			contextData = median;
		}

		public override object Final(object contextData)
		{
			if (contextData == null) return null;
			var median = contextData is double ? (double) contextData : 0; 
			return median;
		}
	}
}