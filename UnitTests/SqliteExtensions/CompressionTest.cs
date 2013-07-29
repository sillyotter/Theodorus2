using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class CompressionTest
    {
        [Fact]
        public void RoundTripTest()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
            var res = SqliteExtensionHelper.ScalarFunction<byte[], byte[]>(() => new Compress(), new[] { bytes });
            var res2 = SqliteExtensionHelper.ScalarFunction<byte[], byte[]>(() => new Decompress(), new[] { res });
            Assert.NotEqual(res, bytes);
            Assert.Equal(res2, bytes);
        }
    }
}