using NUnit.Framework;
using System.Drawing;
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
           var id= Storage.Local.SaveImage(stream, "png");
            Assert.IsNotEmpty(id);

         var img=   Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }


        [Test]
        public void Icon() {
            var stream = Image.FromFile("userMgr.ico");
            var id = Storage.Local.SaveImage(stream, "ico");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }
        [Test]
        public void Bmp()
        {
            var stream = Image.FromFile("100-2.bmp");
            var id = Storage.Local.SaveImage(stream, "bmp");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }


        [Test]
        public void Jpg() {
            var stream = Image.FromFile("splash.jpg");
            var id = Storage.Local.SaveImage(stream, "jpg");
            Assert.IsNotEmpty(id);

            var img = Storage.Local.GetImage(id);
            Assert.AreEqual(stream.Width, img.Width);
            Assert.AreEqual(stream.Height, img.Height);
            Assert.AreEqual(stream.RawFormat, img.RawFormat);
            Assert.Pass();
        }
    }

}