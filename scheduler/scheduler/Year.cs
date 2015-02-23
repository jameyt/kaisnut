using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Year : IYear
    {
        public DateTime Date { get; set; }
        public List<IMonth> Months { get; private set; }

        private Year()
        {

        }

        public static IYear Create(DateTime date)
        {
            var year = new Year();
            year.Date = date;
            year.AddMonths();

            return year;
        }

        private void AddMonths()
        {
            if (Months == null) { Months = new List<IMonth>(); }
            var month = new DateTime(Date.Year, 1, 1);
            while (month.Year == Date.Year)
            {
                Months.Add(Month.Create(month));
                month = month.AddMonths(1);
            }
        }


        public void AddAssignment(IAssignment assignment)
        {
            foreach (var month in Months.Where(month => month.Date.Month == assignment.Date.Month))
            {
                month.AddAssignment(assignment.Date, assignment);
            }
        }

        public List<IAssignment> GetAssignments()
        {
            return Months.SelectMany(month => month.GetAssignments()).ToList();
        }

        public List<IAssignment> GetAssignments(IEmployee employee)
        {
            return (from assignment in GetAssignments()
                    where employee.Equals(assignment.Employee)
                    select assignment).ToList();
        }

        public List<IAssignment> GetAssignments(DateTime date)
        {
            return Months
                .Where(month => month.Date.Month == date.Month)
                .SelectMany(month => month.GetAssignments())
                .ToList();
        }

        public List<IAssignment> GetAssignments(IEmployee employee, DateTime date)
        {
            return GetAssignments(date).Where(assignment => assignment.Employee == employee).ToList();
        }
    }
}
