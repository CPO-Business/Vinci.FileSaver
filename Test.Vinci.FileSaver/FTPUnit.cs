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
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConnectRemoteFTP()
        {
            using (var _Storage = Storage.FTP.Build("ftp://192.168.10.204", "inspack", "123456@q"))
            {
                Assert.IsNotNull(_Storage);
            }

            Assert.Pass();
        }



        [Test]
        public void SaveFileToFTP()
        {
            var id = "";
            long length = 0;
            using (var _Storage = Storage.FTP.Build("ftp://192.168.10.204", "inspack", "123456@q"))
            {
                using (var file = File.OpenRead("userMgr.ico"))
                {
                    length = file.Length;
                    id = _Storage.SaveFile(file, "ico", null, "userMgr");
                }
                Assert.AreEqual("path:userMgr.ico", id);


                var resultStream = _Storage.GetFile(id);
                Assert.IsNotNull(resultStream);
                Assert.AreEqual(length, resultStream.Length);
            }

            Assert.Pass();
        }

        [Test]
        public void SaveFileToFTPWithOtherDir()
        {
            var id = "";
            long length = 0;
            using (var _Storage = Storage.FTP.Build("ftp://192.168.10.204", "inspack", "123456@q"))
            {
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
            }
            Assert.Pass();
        }
    }
}
