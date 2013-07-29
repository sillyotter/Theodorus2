using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Aggregate, Name = "CovarianceP")]
	public class CovarianceP : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			VarianceHelper.CovarianceStep(args, stepNumber, ref contextData);
		}

		public override object Final(object contextData)
		{
			var newData = contextData as Tuple<int, double, double, double>;
			if (newData == null) return null;

			var n = newData.Item1;
			var sum1 = newData.Item2;
			var sum2 = newData.Item3;
			var sum12 = newData.Item4;

			return (sum12 - sum1*sum2/n)/n;
		}
	}
}