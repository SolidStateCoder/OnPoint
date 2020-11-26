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

        [Test]
        public void SplitAtCapitals()
        {
            Assert.AreEqual("ABC".SplitAtCapitals(), "ABC");
            Assert.AreEqual("AxByCz".SplitAtCapitals(), "Ax By Cz");
            Assert.AreEqual("AxxByyCzz".SplitAtCapitals(), "Axx Byy Czz");
            Assert.AreEqual("ABCxyz".SplitAtCapitals(), "AB Cxyz");
            Assert.AreEqual("xyzABC".SplitAtCapitals(), "xyz ABC");
            Assert.AreEqual("aFOO".SplitAtCapitals(), "aFOO");
            Assert.AreEqual("aFoo".SplitAtCapitals(), "aFoo");
        }
    }
}
