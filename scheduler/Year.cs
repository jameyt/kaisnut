using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Year
    {
        public DateTime Date { get; private set; }
        public List<Month> Months { get; private set; }

        private Year()
        {

        }

        public static Year Create(DateTime date)
        {
            var year = new Year();
            year.Date = date;
            year.AddMonths();

            return year;
        }

        private void AddMonths()
        {
            if (Months == null) { Months = new List<Month>(); }
            var month = new DateTime(Date.Year, 1, 1);
            while (month.Year == Date.Year)
            {
                Months.Add(Month.Create(month));
                month = month.AddMonths(1);
            }
        }


        internal void AddAssignment(DateTime date, Assignment assignment)
        {
            foreach (var month in Months)
            {
                if (month.Date.Month == date.Month)
                {
                    month.AddAssignment(date, assignment);
                }
            }
        }

        public List<Assignment> GetAssignments()
        {
            return Months.SelectMany(month => month.GetAssignments()).ToList();
        }

        public List<Assignment> GetAssignments(Employee employee)
        {
            return (from assignment in GetAssignments()
                    where employee.Equals(assignment.Employee)
                    select assignment).ToList();
        }

        public List<Assignment> GetAssignments(DateTime date)
        {
            return Months
                .Where(month => month.Date.Month == date.Month)
                .SelectMany( month => month.GetAssignments())
                .ToList();
        }

        public List<Assignment> GetAssignments(Employee employee, DateTime date)
        {
            return GetAssignments(date).Where(assignment => assignment.Employee == employee).ToList();
        }
    }
}
