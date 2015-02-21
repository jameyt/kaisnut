using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scheduler.data;

namespace scheduler.tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void CreateSchedule()
        {
            var repo = Repository.Create();
            var schedule = Schedule.Create(repo);
        }

        [TestMethod]
        public void CreateVacation()
        {
            var vacation = Vacation.Create(
                new DateTime(2015, 7, 5),
                new DateTime(2015, 7, 19)
                );
        }

        [TestMethod]
        public void CreateEmptyThreeYearSchedule()
        {
            var schedule = Schedule.CreateEmptyThreeYear();
            Assert.AreEqual(3, schedule.Years.Count);
            foreach (var year in schedule.Years)
            {
                Assert.AreEqual(12, year.Months.Count);
                foreach (var month in year.Months)
                {
                    Assert.IsTrue(27 < month.Days.Count);
                }
            }
        }

        [TestMethod]
        public void AddAssignment()
        {
            var schedule = Schedule.CreateEmptyThreeYear();
            var assignment = Assignment.Create();
            assignment.Role = Role.First;
            var employee = new Employee
            {
                Contact = Contact.Create(),
                FirstName = "Joe",
                LastName = "Smith"
            };
            var date = DateTime.Now.AddDays(17);
            employee.Contact.Phone = "555-555-5555";
            employee.Contact.Email = "joe.smith@google.com";
            employee.Contact.Address = "123 S Main St Tulsa OK 74107";
            assignment.Employee = employee;
            schedule.AddAssignment(date, assignment);

            Assert.AreEqual(1,schedule.Years[1].Months[2].Days[9].Assignments.Count);
        }
    }
}