using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class HexTests
    {
        [Fact]
        public void HexEncodeDecodeRoundTripTest()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
            var res = SqliteExtensionHelper.ScalarFunction<byte[], string>(() => new HexEncode(), new[] { bytes });
            var res2 = SqliteExtensionHelper.ScalarFunction<string, byte[]>(() => new HexDecode(), new[] { res });
            Assert.Equal(res2, bytes);
        }
    }
}