using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Schedule
    {
        public Day CurrentDay { get; set; }
        public Month CurrentMonth { get; set; }
        public Year CurrentYear { get; set; }

        public List<Year> Years { get; set; } 

        private Schedule() 
        {
            
        }

        public static Schedule Create()
        {
            return new Schedule();
        }

        public static Schedule CreateEmptyThreeYear()
        {
            var schedule = new Schedule();
            schedule.Years = new List<Year>();
            schedule.Years.Add(Year.Create( DateTime.Now.AddYears(-1)));
            schedule.Years.Add(Year.Create(DateTime.Now));
            schedule.Years.Add(Year.Create(DateTime.Now.AddYears(1)));
            return schedule;
        }

        public void AddAssignment(DateTime date,Assignment assignment)
        {
            foreach (var year in Years)
            {
                if (year.Date.Year == date.Year)
                {
                    year.AddAssignment(date,assignment);
                }
            }
        }
    }
}
