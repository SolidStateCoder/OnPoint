using NUnit.Framework;
using OnPoint.Universal;
using OnPoint.WpfDotNet5;
using System.Globalization;
using System.Windows;

namespace OnPoint.UnitTests
{
    public class WpfDotNet5Tests
    {
        [Test]
        public void WpfEnumTests()
        {
            Assert.AreEqual(Visibility.Visible, Visibility.Collapsed.Invert());
            Assert.AreEqual(Visibility.Visible, Visibility.Hidden.Invert());
            Assert.AreEqual(Visibility.Collapsed, Visibility.Visible.Invert());
            Assert.AreEqual(Visibility.Hidden, Visibility.Visible.Invert(true));

            Assert.AreEqual(WindowState.Normal, AppDisplayState.Normal.ToWindowState());
            Assert.AreEqual(AppDisplayState.Normal, WindowState.Normal.ToAppDisplayState());
        }

        [Test]
        public void ConverterTests()
        {
            BooleanNegatorConverter converter = new BooleanNegatorConverter();
            Assert.IsFalse((bool)converter.Convert(true, null, null, CultureInfo.InvariantCulture));
            Assert.IsTrue((bool)converter.Convert(true, null, null, CultureInfo.InvariantCulture));
        }
    }
}