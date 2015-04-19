using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IRepository
    {
        List<IAssignment> Assignments { get; set; }
        List<IEmployee> Employees { get; set; }
        IEmployee GetEmployeeById(int employeeId);
        IEmployee GetEmployeeByInitials(string employeeInitials);
        void UpdateAssignment(IAssignment assignment);
        void DeleteAssignment(int id);
        void AddAssignment(IAssignment assignment);
        IAssignment GetAssignment(int id);
        IDetailedSchedule GetDetailedSchedule(DateTime date);
    }
}
