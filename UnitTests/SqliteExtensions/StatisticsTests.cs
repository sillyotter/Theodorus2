using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class StatisticsTests
    {
        [Fact]
        public void MedianTest()
        {
            Assert.Equal(2.0, SqliteExtensionHelper.AggregateFunction<int,double>(() => new Median(), new[] {1, 2, 3}));
            Assert.Equal(2.5, SqliteExtensionHelper.AggregateFunction<int,double>(() => new Median(), new[] {1, 2, 3, 4}));
            Assert.Equal(1, SqliteExtensionHelper.AggregateFunction<int, double>(() => new Median(), new[] {1, 1, 1, 1}));
        }

        [Fact]
        public void CovarianceTest()
        {
            Assert.Equal(-2,
                SqliteExtensionHelper.AggregateFunction<int, double>(() => new CovarianceP(),
                    new[] {new[] {1, 2, 3, 4, 5}, new[] {5, 4, 3, 2, 1}}));
        }

        [Fact]
        public void SkewTest()
        {
            Assert.Equal(0, SqliteExtensionHelper.AggregateFunction<int, double>(() => new Skew(), new[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void KurtosisTest()
        {
            Assert.Equal(-1.2, SqliteExtensionHelper.AggregateFunction<int, double>(() => new Kurtosis(), new[] { 1, 2, 3, 4, 5 }), 4);
        }

        [Fact]
        public void VarianceSampTest()
        {
            Assert.Equal(2.5, SqliteExtensionHelper.AggregateFunction<int, double>(() => new VarianceSamp(), new[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void VariancePopTest()
        {
            Assert.Equal(2, SqliteExtensionHelper.AggregateFunction<int, double>(() => new VariancePop(), new[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void StdDevPopTest()
        {
            Assert.Equal(1.41, SqliteExtensionHelper.AggregateFunction<int, double>(() => new StdDevPop(), new[] { 1, 2, 3, 4, 5 }), 2);
        }
        [Fact]
        public void StdDevSampTest()
        {
            Assert.Equal(1.58, SqliteExtensionHelper.AggregateFunction<int, double>(() => new StdDevSamp(), new[] { 1, 2, 3, 4, 5 }), 2);
        }
        
        
    }
}
