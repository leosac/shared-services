using Microsoft.VisualStudio.TestTools.UnitTesting;
using Leosac.SharedServices;

namespace Leosac.SharedServices.Tests
{
    [TestClass]
    public class LeosacAppInfoTests
    {
        private class DummyAppInfo : LeosacAppInfo { public DummyAppInfo() : base() { ApplicationName = "Dummy"; ApplicationTitle = "Dummy Title"; ApplicationCode = "DUMMY"; PerUserInstallation = true; } }

        [TestMethod]
        public void Instance_CanBeSet_And_ReadProperties()
        {
            var dummy = new DummyAppInfo();
            LeosacAppInfo.Instance = dummy;
            Assert.IsNotNull(LeosacAppInfo.Instance);
            Assert.AreEqual("Dummy", LeosacAppInfo.Instance!.ApplicationName);
            Assert.AreEqual("Dummy Title", LeosacAppInfo.Instance.ApplicationTitle);
            Assert.AreEqual("DUMMY", LeosacAppInfo.Instance.ApplicationCode);
        }
    }
}
