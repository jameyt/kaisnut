using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Month : IMonth
    {
        public DateTime Date { get; set; }
        public List<IDay> Days { get; set; }
        public string Name { get; set; }
        public List<IWeek> Weeks { get; set; }

        private Month() { }

        public static IMonth Create(DateTime date)
        {
            var month = new Month();
            month.Date = date;
            month.AddDays();
            month.LoadWeeks();
            return month;
        }

        private void AddDays()
        {
            if (Days == null) { Days = new List<IDay>(); }
            var day = new DateTime(Date.Year, Date.Month, 1);
            while (day.Month == Date.Month)
            {
                Days.Add(Day.Create(null, day));
                day = day.AddDays(1);
            }
        }

        public void AddAssignment(DateTime date, IAssignment assignment)
        {
            foreach (var day in Days.Where(day => day.Date.Day == date.Day))
            {
                day.Assignments.Add(assignment);
            }
        }

        public List<IAssignment> GetAssignments()
        {
            return Days.SelectMany(day => day.Assignments).ToList();
        }

        public List<IAssignment> GetAssignments(IEmployee employee)
        {
            return (from assignment in GetAssignments()
                    where employee.Equals(assignment.Employee)
                    select assignment).ToList();
        }

        public List<IAssignment> GetAssignments(DateTime date)
        {
            return (from day in Days
                    where date.Date == day.Date.Date
                    select day.Assignments).FirstOrDefault();
        }

        public List<IAssignment> GetAssignments(IEmployee employee, DateTime date)
        {
            return GetAssignments(date).Where(assignment => assignment.Employee == employee).ToList();
        }

        public void SetName(int id)
        {
            switch (id)
            {
                case 0:
                    Name = "January";
                    break;
                case 1:
                    Name = "February";
                    break;
                case 2:
                    Name = "March";
                    break;
                case 3:
                    Name = "April";
                    break;
                case 4:
                    Name = "May";
                    break;
                case 5:
                    Name = "June";
                    break;
                case 6:
                    Name = "July";
                    break;
                case 7:
                    Name = "August";
                    break;
                case 8:
                    Name = "September";
                    break;
                case 9:
                    Name = "October";
                    break;
                case 10:
                    Name = "November";
                    break;
                case 11:
                    Name = "December";
                    break;
            }
        }

        private void LoadWeeks()
        {
            const bool firstSunday = false;
            Weeks = new List<IWeek>();

            var k = GetDayOfWeekOffset();
            for (var i = 0; i < 6; i++)
            {
                var week = new Week();
                week.Days = new List<IDay>();
                for (var j = 0; j < 7; j++)
                {
                    var x = j + i * 7 - k;
                    if (x < 0 || x >= Days.Count) 
                    {
                        week.Days.Add(Day.CreateEmpty());
                        continue;
                    }
                    var day = Days[x];
                    week.Days.Add(day);
                }
                Weeks.Add(week);
            }
        }

        private int GetDayOfWeekOffset()
        {
            var offset = 0;

            switch (Days[0].Date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                default:
                    return 0;
            }

        }
    }
}
