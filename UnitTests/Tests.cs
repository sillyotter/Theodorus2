using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Tests
    {
        [Fact]
        public async Task Test1()
        {
            await Task.Delay(100);
            Assert.True(true);
        }
    }
}
