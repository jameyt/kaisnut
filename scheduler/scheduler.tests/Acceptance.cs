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
            var connectionString =
                "Server=tcp:dzvmbj8x0g.database.windows.net,1433;Database=mercycrna;User ID=mercycrnadata@dzvmbj8x0g;Password=Coconut12;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var cn = DataInteraction.CreateSqlConnection("tcp:dzvmbj8x0g.database.windows.net,1433","mercycrna","mercycrnadata","Coconut12");
            var repo = Repository.Create(cn);
            repo.Seed();
            repo.SaveEmployees( );
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
