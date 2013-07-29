using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class EncryptDecrypt
    {
        [Fact]
        public void EncryptDecryptTest()
        {
            var src = new byte[] {1, 2, 3, 4, 5, 6, 7};
            var key = new byte[] {9, 0, 0, 9};
            var enc = SqliteExtensionHelper.ScalarFunction<byte[], byte[]>(() => new Encrypt(), new[] {src, key});
            var dec = SqliteExtensionHelper.ScalarFunction<byte[], byte[]>(() => new Decrypt(), new[] {enc, key});
            Assert.NotEqual(enc, src);
            Assert.Equal(dec, src);

        }
    }
}