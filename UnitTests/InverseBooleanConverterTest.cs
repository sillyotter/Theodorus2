using System;
using Theodorus2.Support;
using Xunit;

namespace UnitTests
{
    public class InverseBooleanConverterTest
    {
        [Fact]
        public void TestConverter()
        {
            var cvt = new InverseBooleanConverter();
            Assert.Equal(false, cvt.Convert(true, typeof (Boolean), null, null));
            Assert.Equal(true, cvt.Convert(false, typeof(Boolean), null, null));

            Assert.Equal(true, cvt.ConvertBack(false, typeof(Boolean), null, null));
            Assert.Equal(true, cvt.ConvertBack(false, typeof(Boolean), null, null));

        }
    }
}