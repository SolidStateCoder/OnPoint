using NUnit.Framework;
using OnPoint.Universal;
using OnPoint.WpfCore;
using System.Windows;

namespace OnPoint.UnitTests
{
    public class WpfCoreTests
    {
        [Test]
        public void WpfTests()
        {
            Assert.AreEqual(Visibility.Visible, Visibility.Collapsed.Invert());
            Assert.AreEqual(Visibility.Visible, Visibility.Hidden.Invert());
            Assert.AreEqual(Visibility.Collapsed, Visibility.Visible.Invert());
            Assert.AreEqual(Visibility.Hidden, Visibility.Visible.Invert(true));

            Assert.AreEqual(WindowState.Normal, AppDisplayState.Normal.ToWindowState());
            Assert.AreEqual(AppDisplayState.Normal, WindowState.Normal.ToAppDisplayState());
        }
    }
}