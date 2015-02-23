using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IDay
    {
        List<Role> Roles { get; }

        List<IAssignment> Assignments { get; }

        List<IEmployee> Employees { get; }

        DateTime Date { get; }

        List<IAssignment> GetAssignmentsByEmployee(IEmployee employee);
    }
}
