using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vinci.FileSaver;
using Vinci.FileSaver.Interface;

namespace Test.Vinci.FileSaver
{
    public class UnitFTPHelper
    {
        private IStorage _Storage;
        [SetUp]
        public void Setup()
        {
            _Storage = Storage.FTP.Build("ftp://192.168.10.204", "inspack", "123456@q");
        }


        [Test]
        public void List()
        {

            FTPHelper.Clear(_Storage);
            using (var file = File.OpenRead("userMgr.ico"))
            {
                _Storage.SaveFile(file, "ico", null, $"newAdd_1");
                _Storage.SaveFile(file, "ico", null, $"newAdd_2");
                _Storage.SaveFile(file, "ico", null, $"newAdd_3");
            }

            var strs = FTPHelper.ListFile(_Storage);
            Assert.AreEqual(3, strs.Length);
            foreach (var name in strs)
            {
                Assert.IsTrue(name.StartsWith("newAdd_"));
                Assert.IsTrue(name.EndsWith(".ico"));
            }
            Assert.Pass();
        }
    }
}
