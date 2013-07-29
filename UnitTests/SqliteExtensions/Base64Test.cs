using System;
using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class Base64Test
    {
        [Fact]
        public void EncodeTest()
        {
            var bytes = new byte[] {1, 2, 3, 4, 5, 6, 7};
            var res = SqliteExtensionHelper.ScalarFunction<byte[], string>(() => new Base64Encode(), new[]{bytes});
            var res2 = Convert.ToBase64String(bytes);
            Assert.Equal(res2, res);
        }

        [Fact]
        public void DecodeTest()
        {
            var encoded = Convert.ToBase64String(new byte[] {1, 2, 3, 4, 5, 6, 7});
            var res = SqliteExtensionHelper.ScalarFunction<string, byte[]>(() => new Base64Decode(), new[] { encoded });
            var res2 = Convert.FromBase64String(encoded);
            Assert.Equal(res2, res);
        }

        [Fact]
        public void RoundTripTest()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
            var res = SqliteExtensionHelper.ScalarFunction<byte[],string>(() => new Base64Encode(), new[] { bytes });
            var res2 = SqliteExtensionHelper.ScalarFunction<string, byte[]>(() => new Base64Decode(), new[] { res });
            Assert.Equal(res2, bytes);
        }
    }

    
}