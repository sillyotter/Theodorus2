using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Aggregate, Name="Kurtosis")]
	public class Kurtosis : SQLiteFunction
	{
		public override void Step(object[] args, int stepNumber, ref object contextData)
		{
			dynamic item = args[0];
			var newData = contextData as Tuple<int, double, double, double, double> ?? Tuple.Create(0, 0.0, 0.0, 0.0, 0.0);

			var n = newData.Item1;
			var mean = newData.Item2;
			var m2 = newData.Item3;
			var m3 = newData.Item4;
			var m4 = newData.Item5;

			var n1 = n;
			n = n + 1;
			var delta = item - mean;
			var deltaN = delta/n;
			var deltaN2 = deltaN*deltaN;
			var term1 = delta * deltaN * n1;
			mean = mean + deltaN;
			m4 = m4 + term1*deltaN2*(n*n - 3*n + 3) + 6*deltaN2*m2 - 4*deltaN*m3;
			m3 = m3 + term1*deltaN*(n - 2) - 3*deltaN*m2;
			m2 = m2 + term1;

			contextData = Tuple.Create(n, mean, m2, m3, m4);
		}

		public override object Final(object contextData)
		{
			var data = contextData as Tuple<int, double, double, double, double>;
			if (data == null) return null;

			var n = data.Item1;
			var m2 = data.Item3;
			var m4 = data.Item5;

			return (n*m4)/(m2*m2) - 3;
		}
	}
}

// Need a skewness test.  wikipedia has an algo, but im not sure yet how to implement it.  maybe apacke commons math has something worth stealing
