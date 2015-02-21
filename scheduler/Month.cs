using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Month
    {
        public DateTime Date { get; set; }
        public List<Day> Days { get; set; }

        private Month() { }

        public static Month Create(DateTime date)
        {
            var month = new Month();
            month.Date = date;
            month.AddDays();
            return month;
        }

        private void AddDays()
        {
            if (Days == null) { Days = new List<Day>(); }
            var day = new DateTime(Date.Year, Date.Month, 1);
            while (day.Month == Date.Month)
            {
                Days.Add(Day.Create(null, day));
                day = day.AddDays(1);
            }
        }

        internal void AddAssignment(DateTime date, Assignment assignment)
        {
            foreach (var day in Days.Where(day => day.Date.Day == date.Day))
            {
                day.Assignments.Add(assignment);
            }
        }

        public List<Assignment> GetAssignments()
        {
            return Days.SelectMany(day => day.Assignments).ToList();
        }

        public List<Assignment> GetAssignments(Employee employee)
        {
            return (from assignment in GetAssignments()
                    where employee.Equals(assignment.Employee)
                    select assignment).ToList();
        }

        public List<Assignment> GetAssignments(DateTime date)
        {
            return (from day in Days
                    where date.Date == day.Date.Date
                    select day.Assignments).FirstOrDefault();
        }

        public List<Assignment> GetAssignments(Employee employee, DateTime date)
        {
            return GetAssignments(date).Where(assignment => assignment.Employee == employee).ToList();
        }
    }
}
