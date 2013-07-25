using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name = "VarP")]
	public class VariancePop : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			VarianceHelper.StdDevStep(args, stepNumber, ref contextData);
		}

		public override object Final(object contextData)
		{
			var data = contextData as Tuple<int, double, double>;
			if (data == null) return null;

			var n = data.Item1;
			var s = data.Item3;

			return (s / n);
		}
	}
}