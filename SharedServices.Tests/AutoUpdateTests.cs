using Microsoft.VisualStudio.TestTools.UnitTesting;
using Leosac.SharedServices;

namespace Leosac.SharedServices.Tests
{
    [TestClass]
    public class AutoUpdateTests
    {
        [TestMethod]
        public void Default_HasUpdate_IsFalse()
        {
            var au = new AutoUpdate();
            Assert.IsFalse(au.HasUpdate);
            Assert.IsNull(au.UpdateVersion);
        }

        [TestMethod]
        public void DownloadUpdate_DoesNotThrow_WhenNoUri()
        {
            var au = new AutoUpdate();
            au.UpdateVersion = new UpdateVersion { VersionString = "0.0.1", Uri = null };
            au.DownloadUpdate(); // should not throw
        }
    }
}
