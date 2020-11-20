using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vinci.FileSaver;
using Vinci.FileSaver.Interface;

namespace Test.Vinci.FileSaver
{
    public class FTPUnit
    {
        private IStorage _Storage;
        [SetUp]
        public void Setup()
        {
            _Storage = Storage.FTP.Build("ftp://192.168.10.204", "inspack", "123456@q");
        }

        [Test]
        public void ConnectRemoteFTP()
        {
            Assert.IsNotNull(_Storage);
            Assert.Pass();
        }



        [Test]
        public void SaveFileToFTP()
        {
            var id = "";
            long length = 0;
            using (var file = File.OpenRead("userMgr.ico"))
            {
                length = file.Length;
                id = _Storage.SaveFile(file, "ico", null, "userMgr");
            }
            Assert.AreEqual("path:userMgr.ico", id);


            var resultStream = _Storage.GetFile(id);
            Assert.IsNotNull(resultStream);
            Assert.AreEqual(length, resultStream.Length);
            Assert.Pass();
        }

        [Test]
        public void SaveFileToFTPWithOtherDir()
        {
            var id = "";
            long length = 0;
            var dir = new DirectoryInfo("FileSaver.FTPStorage");
            using (var file = File.OpenRead("userMgr.ico"))
            {
                length = file.Length;
                id = _Storage.SaveFile(file, "ico", dir, "userMgr");
            }
            Assert.AreEqual("path:userMgr.ico", id);


            var resultStream = _Storage.GetFile(id, dir);
            Assert.IsNotNull(resultStream);
            Assert.AreEqual(length, resultStream.Length);
            Assert.Pass();
        }
    }
}
