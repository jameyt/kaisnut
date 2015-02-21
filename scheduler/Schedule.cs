using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.data;

namespace scheduler
{
    public class Schedule:ISchedule
    {
        private IRepository repo;

        //public Day CurrentDay { get; set; }
        //public Month CurrentMonth { get; set; }
        //public Year CurrentYear { get; set; }

        public List<Year> Years { get; set; }

        private Schedule()
        {

        }

        public static Schedule Create(IRepository repository)
        {
            return new Schedule()
            {
                repo = repository
            };
        }

        public static Schedule CreateEmptyThreeYear()
        {
            var schedule = new Schedule
            {
                Years = new List<Year>
                {
                    Year.Create(DateTime.Now.AddYears(-1)),
                    Year.Create(DateTime.Now),
                    Year.Create(DateTime.Now.AddYears(1))
                }
            };
            return schedule;
        }

        public static Schedule CreateEmptyThisYear()
        {
            var schedule = new Schedule
            {
                Years = new List<Year>
                {
                    Year.Create(DateTime.Now)
                 }
            };
            return schedule;
        }

        public void AddAssignment(Assignment assignment)
        {
            foreach (var year in Years.Where(year => year.Date.Year == assignment.Date.Year))
            {
                year.AddAssignment(assignment.Date, assignment);
            }
        }
    }
}
