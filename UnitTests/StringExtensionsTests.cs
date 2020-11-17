using NUnit.Framework;
using OnPoint.Universal;

namespace OnPoint.UnitTests
{
    public class StringExtensionsTests
    {
        private string _Colors = "Red.White.Blue.";
        private string _Null = null;

        [Test]
        public void Substrings()
        {
            Assert.AreEqual(_Colors.EverythingAfterFirst("."), "White.Blue.");
            Assert.AreEqual(_Colors.EverythingAfterLast("."), "");
            Assert.AreEqual(_Colors.EverythingAfterLast("e"), ".");
            Assert.AreEqual(_Colors.EverythingAfterFirst("z"), string.Empty);
            Assert.AreEqual(_Colors.EverythingAfterLast("z"), string.Empty);
            Assert.AreEqual(_Null.EverythingAfterFirst("z"), null);
            Assert.AreEqual(_Null.EverythingAfterLast("z"), null);
        }
    }
}
