using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vinci.FileSaver.Extension;

namespace Test.Vinci.FileSaver
{
    public class DirectoryTests
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void CopyTo()
        {
            var testDir = Directory.CreateDirectory("testDir");
            var f1 = File.Create(Path.Combine(testDir.FullName, "t1.text"), 20);
            f1.Close();
            var subDir = testDir.CreateSubdirectory("subDir");
            var f2 = File.Create(Path.Combine(subDir.FullName, "t2.text"), 20);
            f2.Close();
            testDir.CopyTo("destDir", true);
            var destDir = new DirectoryInfo("destDir");
            Assert.IsTrue(destDir.Exists);
            subDir = destDir.GetDirectories()[0];
            Assert.AreEqual("subDir", subDir.Name);
            Assert.AreEqual(destDir.GetFiles()[0].Name, "t1.text");
            Assert.AreEqual(subDir.GetFiles()[0].Name, "t2.text");
            destDir.Delete(true);
            testDir.Delete(true);
            Assert.Pass();
        }
    }
}
