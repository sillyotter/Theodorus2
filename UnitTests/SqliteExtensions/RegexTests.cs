using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class RegexTests
    {
        [Fact]
        public void IsMatchTest()
        {
            Assert.True(SqliteExtensionHelper.ScalarFunction<string, bool>(() => new IsMatch(),
                new[] {"ThisIsTheString", "This.*String"}));
            Assert.False(SqliteExtensionHelper.ScalarFunction<string, bool>(() => new IsMatch(),
                new[] { "ThisIsTheString", "XXs.*ng" }));
        }

        [Fact]
        public void ReplaceTest()
        {
            Assert.Equal("this is a test",SqliteExtensionHelper.ScalarFunction<string, string>(() => new Replace(),
                new[] {"this is the test", "the", "a"}));
            Assert.Equal("this is the test", SqliteExtensionHelper.ScalarFunction<string, string>(() => new Replace(),
                new[] { "this is the test", "xxx", "a" }));
        }

    }
}