using System;
using System.Data.SqlClient;
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
            var cs = @"Data Source=(LocalDB)\v11.0;
                          AttachDbFilename=C:\Users\tyler-eg\Source\Repos\kaisnut\scheduler.tests\local.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;";
            var cn = new SqlConnection(cs);
            cn.Open();
            var repo = Repository.Create(cn);
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

            var employee = Employee.Create(
              "Joe",
              "Smith",
              DateTime.Now,
              Contact.Create(
                  "555-555-5555",
                  "joe.smith@google.com",
                  "123 S Main St Tulsa OK 74107"
                  ));

            var assignment = Assignment.Create(Role.First,
                employee,
                new DateTime(2015, 3, 1));
            assignment.Role = Role.First;

            employee.Contact.Phone = "555-555-5555";
            employee.Contact.Email = "joe.smith@google.com";
            employee.Contact.Address = "123 S Main St Tulsa OK 74107";
            assignment.Employee = employee;
            schedule.AddAssignment(assignment);

            Assert.AreEqual(1, schedule.Years[1].Months[2].Days[0].Assignments.Count);
        }
    }
}