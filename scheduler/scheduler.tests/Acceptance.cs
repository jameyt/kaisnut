using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scheduler.data;
using utilities.data;

namespace scheduler.tests
{
    /// <summary>
    /// Summary description for Acceptance
    /// </summary>
    [TestClass]
    public class Acceptance
    {
        [TestMethod]
        public void CreateSchedule()
        {
            const string databaseLocation = @"C:\Users\tyler-eg\Source\Repos\kaisnut\scheduler.tests\local.mdf";
            var cn = DataInteraction.CreateLocalSqlConnection(databaseLocation);
            var repo = Repository.Create(cn);
            var schedule = Schedule.Create(repo);
        }

        [TestMethod]
        public void ThisIsTrue()
        {


            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TheseAreEqual()
        {


            Assert.AreEqual(true, false);
        }
    }
}
