using System;

namespace Theodorus2.SqliteExtensions
{
	// http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance

	internal static class VarianceHelper
	{
		public static void StdDevStep(object[] args, int stepNumber, ref object contextData)
		{
			dynamic item = args[0];
			var newData = contextData as Tuple<int, double, double> ?? Tuple.Create(0, 0.0, 0.0);

			var k = newData.Item1;
			var m = newData.Item2;
			var s = newData.Item3;

			k = k + 1;
			var delta = item - m;
			m = m + delta / k;
			s = s + delta * (item - m);

			contextData = Tuple.Create(k, m, s);
		}

		public static void CovarianceStep(object[] args, int stepNumber, ref object contextData)
		{
			dynamic item1 = args[0];
			dynamic item2 = args[1];

			var newData = contextData as Tuple<int, double, double, double> ?? Tuple.Create(0, 0.0, 0.0, 0.0);

			var n = newData.Item1;
			var sum1 = newData.Item2;
			var sum2 = newData.Item3;
			var sum12 = newData.Item4;

			sum1 += item1;
			sum2 += item2;
			sum12 += item1 * item2;

			contextData = Tuple.Create(n + 1, sum1, sum2, sum12);
		}

		public static void SkewStep(object[] args, int stepNumber, ref object contextData)
		{
			dynamic v = args[0];

			var newData = contextData as Tuple<int, double, double, double> ?? Tuple.Create(0, 0.0, 0.0, 0.0);

			var n = newData.Item1;
			var sum1 = newData.Item2;
			var sum2 = newData.Item3;
			var sum3 = newData.Item4;

			var ov = v;
			sum1 += v;
			v *= ov;
			sum2 += v;
			v *= ov;
			sum3 += v;
			
			contextData = Tuple.Create(n + 1, sum1, sum2, sum3);
		}

	    public static void SnKStep(object[] args, int stepNumber, ref object contextData)
	    {
            dynamic xi = args[0];
            var newData = contextData as Tuple<int, double, double, double, double> ?? Tuple.Create(0, 0.0, 0.0, 0.0, 0.0);
            var n = newData.Item1;
            var mean = newData.Item2;
            var variance = newData.Item3;
            var skewness = newData.Item4;
            var kurtosis = newData.Item5;

            double delta = xi - mean;
            var scaleDelta = delta / ++n;
            var scaleDeltaSqr = scaleDelta * scaleDelta;
            var tmpDelta = delta * (n - 1);

            mean += scaleDelta;

            kurtosis += tmpDelta * scaleDelta * scaleDeltaSqr * (n * n - 3 * n + 3)
                + 6 * scaleDeltaSqr * variance - 4 * scaleDelta * skewness;

            skewness += tmpDelta * scaleDeltaSqr * (n - 2) - 3 * scaleDelta * variance;
            variance += tmpDelta * scaleDelta;

            contextData = Tuple.Create(n, mean, variance, skewness, kurtosis);
	    }

	}
}