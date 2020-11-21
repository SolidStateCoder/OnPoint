using NUnit.Framework;
using System.Reactive.Disposables;
using System.Reactive;
using ReactiveUI;
using System;
using System.Linq;

namespace OnPoint.UnitTests
{
    public class ViewModelTests
    {
        private RefreshableVM _RefreshableVM = null;

        [SetUp]
        public void Setup()
        {
            _RefreshableVM = new RefreshableVM() { Title = "Foo" };
        }

        [Test]
        public void Properties()
        {
            Assert.AreEqual(_RefreshableVM.Content, "0");

            Assert.IsFalse(_RefreshableVM.IsCancelEnabled);
            Assert.IsFalse(_RefreshableVM.IsActivated);
            _RefreshableVM.RequestActivated(new CompositeDisposable());
            Assert.IsTrue(_RefreshableVM.IsActivated);
            Assert.IsTrue(_RefreshableVM.IsCancelEnabled);

            _RefreshableVM.HUDMessage = "test";
            Assert.IsTrue(_RefreshableVM.IsShowingHUDMessage);

            Assert.AreEqual(_RefreshableVM.ToString(), "Foo"); // Title

            _RefreshableVM.StartRefresh();
            Assert.IsTrue(_RefreshableVM.IsRefreshing);
            Assert.IsTrue(_RefreshableVM.IsBusy);
            Assert.IsFalse(_RefreshableVM.IsEnabled);

            // TODO: refactor
            while(_RefreshableVM.IsBusy)
            {
                System.Threading.Thread.Yield();
                System.Threading.Thread.Sleep(100);
            }
            Assert.AreEqual(_RefreshableVM.Content, "1");
        }
    }
}