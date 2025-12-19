using Microsoft.VisualStudio.TestTools.UnitTesting;
using Leosac.SharedServices;
using System.IO;

namespace Leosac.SharedServices.Tests
{
    [TestClass]
    public class PermanentConfigTests
    {
        private class TestConfig : PermanentConfig<TestConfig>
        {
            public string Data { get; set; } = "default";
        }

        [TestMethod]
        public void GetDefaultFileName_ReturnsExpected()
        {
            Assert.AreEqual("AppSettings.json", AppSettings.GetDefaultFileName());
            Assert.AreEqual("TestConfig.json", TestConfig.GetDefaultFileName());
        }

        [TestMethod]
        public void SaveToFile_WritesFile_And_LoadFromFile_ReturnsInstance()
        {
            var temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(temp);
            try
            {
                TestConfig.ConfigDirectory = temp;
                var cfg = new TestConfig { Data = "hello" };
                var saved = cfg.SaveToFile();
                Assert.IsTrue(saved);
                var filePath = Path.Combine(temp, TestConfig.GetDefaultFileName());
                Assert.IsTrue(File.Exists(filePath));
                var loaded = TestConfig.LoadFromFile(filePath);
                Assert.IsNotNull(loaded);
                Assert.AreEqual("hello", loaded!.Data);
            }
            finally
            {
                TestConfig.ConfigDirectory = null;
                try { Directory.Delete(temp, true); } catch { }
            }
        }

        [TestMethod]
        public void GetConfigFilePath_Uses_ConfigDirectory()
        {
            var temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(temp);
            try
            {
                TestConfig.ConfigDirectory = temp;
                var path = TestConfig.GetConfigFilePath("my.json", false, false);
                Assert.AreEqual(Path.Combine(temp, "my.json"), path);
            }
            finally { TestConfig.ConfigDirectory = null; try { Directory.Delete(temp, true); } catch { } }
        }
    }
}
