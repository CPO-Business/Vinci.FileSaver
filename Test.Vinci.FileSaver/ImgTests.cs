using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using Vinci.FileSaver;

namespace Test.Vinci.FileSaver
{
    public class ImgTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Png()
        {
            var stream = Image.FromFile("camera.png");
            var id = Storage.Local.SaveImage(stream, "png");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }
        string idToUpdate = string.Empty;

        [Test]
        public void Icon()
        {
            var stream = Image.FromFile("userMgr.ico");
            var id = Storage.Local.SaveImage(stream, "ico");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            //ico is png actually
            Assert.AreEqual(ImageFormat.Png, img.RawFormat);
            Assert.Pass();
        }
        [Test]
        public void Bmp()
        {
            var stream = Image.FromFile("100-2.bmp");
            var idToUpdate = Storage.Local.SaveImage(stream, "bmp");
            Assert.IsNotEmpty(idToUpdate);

            var img = Storage.Local.GetImage(idToUpdate);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }


        [Test]
        public void Jpg()
        {
            var stream = Image.FromFile("splash.jpg");
            var id = Storage.Local.SaveImage(stream, "jpg");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }

        [Test]
        public void UpdateImage()
        {
            if (string.IsNullOrWhiteSpace(idToUpdate))
            {
                Bmp();
            }
            var stream = Image.FromFile("100-2.bmp");

            Storage.Local.UpdateImage(idToUpdate, stream, "bmp");
            var img = Storage.Local.GetImage(idToUpdate);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);


            stream = Image.FromFile("updating.bmp");
            Storage.Local.UpdateImage(idToUpdate, stream, "bmp");
            img = Storage.Local.GetImage(idToUpdate);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);

            Assert.Pass();
        }

        [Test]
        public void SaveImgCostom()
        {
            var dir = new DirectoryInfo("123");
            string path;
            using (var stream = Image.FromFile("100-2.bmp"))
            {
                //"bmp\\" + Guid.NewGuid()
                path = Storage.Local.SaveImage(stream, "bmp", dir, "bmp\\" + Guid.NewGuid());

                Assert.IsNotEmpty(path);

                var img = Storage.Local.GetImage(path, dir);
                Assert.AreEqual(stream.Width, img.Width);
                Assert.AreEqual(stream.Height, img.Height);
                Assert.AreEqual(stream.RawFormat, img.RawFormat);
            }
            Assert.Pass();
        }

        [Test]
        public void UpdateImageCostom()
        {
            var dir = new DirectoryInfo("123");
            string path;
            using (var stream = Image.FromFile("100-2.bmp"))
            {
                path = Storage.Local.SaveImage(stream, "bmp", dir, "bmp\\" + Guid.NewGuid());
            }
            using (var stream = Image.FromFile("updating.bmp"))
            {
                Storage.Local.UpdateImage(path, stream, "bmp", dir);
                var img = Storage.Local.GetImage(path, dir);
                Assert.AreEqual(stream.Width, img.Width);
                Assert.AreEqual(stream.Height, img.Height);
                Assert.AreEqual(stream.RawFormat, img.RawFormat);
            }
            Assert.Pass();
        }

        [Test]
        public void Get_SaveFile()
        {
            string path;
            using (var fs = new FileStream("100-2.bmp", FileMode.Open, FileAccess.Read))
            {
                var id = Storage.Local.SaveFile(fs, "bmp");
                using (var sFs = Storage.Local.GetFile(id))
                {
                    Assert.AreEqual(fs.Length, sFs.Length);
                }
                var dir = new DirectoryInfo("123");
                fs.Seek(0, SeekOrigin.Begin);
                id = Storage.Local.SaveFile(fs, "bmp", dir, "file\\" + Guid.NewGuid());
                using (var sFs = Storage.Local.GetFile(id, dir))
                {
                    Assert.AreEqual(fs.Length, sFs.Length);
                }
            }
            Assert.Pass();
        }
        [Test]
        public void MoveFile()
        {
            string id;
            using (var fs = new FileStream("100-2.bmp", FileMode.Open, FileAccess.Read))
            {
                id = Storage.Local.SaveFile(fs, "bmp");
                var otherId = Storage.PathHelper.GetPathId();
                Storage.Local.MoveFile(id, $"id:{otherId}");
                Assert.Throws(typeof(FileNotFoundException), new TestDelegate(() => { Storage.Local.GetFile(id); }));
                using (var sFs2 = Storage.Local.GetFile(otherId))
                {
                    Assert.AreEqual(sFs2.Length, fs.Length);
                }
            }

            Assert.Pass();
        }

        [Test]
        public void CopyFile()
        {
            string id;
            using (var fs = new FileStream("100-2.bmp", FileMode.Open, FileAccess.Read))
            {
                id = Storage.Local.SaveFile(fs, "bmp");
            }
            var otherId = Storage.PathHelper.GetPathId();
            Storage.Local.CopyFile(id, otherId);
            using (var sFs = Storage.Local.GetFile(id))
            {
                using (var sFs2 = Storage.Local.GetFile(otherId))
                {
                    Assert.AreEqual(sFs2.Length, sFs.Length);
                }
            }
            Assert.Pass();
        }

    }

}