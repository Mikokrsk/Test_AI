using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_AI.Tests
{
    [TestClass()]
    public class PictureConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var converter = new PictureConverter();
            var inputs = converter.Convert(@"D:\Test_AI_C#\Test_AI\Test_AITests\Images\Parasitized.png");
            converter.Save(@"D:\\image.png", inputs);
        }
    }
}