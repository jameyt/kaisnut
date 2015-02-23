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

        public List<IEmployee> Employees { get; set; }

        private Schedule()
        {

        }

        public static ISchedule Create(IRepository repository)
        {
            var schedule = new Schedule {repo = repository};
            schedule.InitializeThreeYear();
            schedule.AddAssignments(repository.Assignments);
            schedule.Employees = repository.Employees;
            
            return schedule;
        }

        public static ISchedule CreateEmptyThreeYear()
        {
            var schedule = new Schedule();
            schedule.InitializeThreeYear();
            return schedule;
        }

        public static ISchedule CreateEmptyThisYear()
        {
            var schedule = new Schedule();
            schedule.InitializeCurrentYear();
            return schedule;
        }

        public void InitializeCurrentYear()
        {
            Years = new List<IYear>
            {
                Year.Create(DateTime.Now)
            };
        }

        public void InitializeThreeYear()
        {
            Years = new List<IYear>
            {
                Year.Create(DateTime.Now.AddYears(-1)),
                Year.Create(DateTime.Now),
                Year.Create(DateTime.Now.AddYears(1))
            };
        }

        public void AddAssignments(List<IAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                AddAssignment(assignment);
            }
        }

        public void AddAssignment(IAssignment assignment)
        {
            foreach (var year in Years.Where(year => year.Date.Year == assignment.Date.Year))
            {
                year.AddAssignment( assignment);
            }
        }

        public List<IAssignment> GetAssignment(DateTime date)
        {
            return Years.SelectMany(year => year.GetAssignments(date)).ToList();
        }

        public List<IAssignment> GetAssignment(IEmployee employee)
        {
            return Years.SelectMany(year => year.GetAssignments(employee)).ToList();
        }
    }
}
