using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name = "SkewP")]
	public class SkewPop : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			VarianceHelper.SkewStep(args, stepNumber, ref contextData);
		}

		public override object Final(object contextData)
		{
			var data = contextData as Tuple<int, double, double, double>;
			if (data == null) return null;

			var n = data.Item1;
			var sum1 = data.Item2;
			var sum2 = data.Item2;
			var sum3 = data.Item2;

			if (n < 3) return null;

			var m1 = sum1 / n;
			var m2 = sum2 / n;
			var m3 = sum3 / n;

			var k2 = m2 - Math.Pow(m1, 2.0);
			var k3 = 2.0*Math.Pow(m1, 3.0) - 3.0*m1*m2 + m3;
			
			var den = Math.Sign(k2)* Math.Pow(Math.Abs(k2), 1.5);
			if (den == 0) return null;

			return (k3/den);
		}
	}
}