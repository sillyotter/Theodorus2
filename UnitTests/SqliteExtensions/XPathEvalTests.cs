using Theodorus2.SqliteExtensions;
using Xunit;

namespace UnitTests.SqliteExtensions
{
    public class XPathEvalTests
    {
        [Fact]
        public void TestEvaluate()
        {
            Assert.Equal("test", SqliteExtensionHelper.ScalarFunction<string, string>(() => new XPathEvaluate(),
                new[] {"<A1><A2><b>test</b></A2></A1>", "//b/text()"}));
        }
    }
}