using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace scheduler.tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void CreateSchedule()
        {
            var schedule = Schedule.Create();
        }

        [TestMethod]
        public void CreateVacation()
        {
            var vacation = Vacation.Create(
                new DateTime(2015, 7, 5),
                new DateTime(2015, 7, 19)
                );
        }
    }
}
