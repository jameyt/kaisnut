using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace draftday.tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void CreatesDraft()
        {
            var draft = Draft.Create();
        }

        
    }
}
