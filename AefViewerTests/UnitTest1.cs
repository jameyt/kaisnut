using System;
using AAON.Ecat.Orders.Data.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AefViewerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateConnection()
        {
            var dataTables = AaonEcatFile.ImportOrder(@"C:\Users\tyler-eg\Desktop\WRL Design.aef");
            var aef = AaonEcatFile.Create(dataTables);
            
        }
    }
}
