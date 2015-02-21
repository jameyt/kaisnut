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

        public List<IYear> Years { get; set; }

        private Schedule()
        {

        }

        public static ISchedule Create(IRepository repository)
        {
            return new Schedule()
            {
                repo = repository
            };
        }

        public static ISchedule CreateEmptyThreeYear()
        {
            var schedule = new Schedule
            {
                Years = new List<IYear>
                {
                    Year.Create(DateTime.Now.AddYears(-1)),
                    Year.Create(DateTime.Now),
                    Year.Create(DateTime.Now.AddYears(1))
                }
            };
            return schedule;
        }

        public static ISchedule CreateEmptyThisYear()
        {
            var schedule = new Schedule
            {
                Years = new List<IYear>
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
                year.AddAssignment( assignment);
            }
        }

        public void GetAssignment(DateTime date)
        {
            
        }
    }
}
