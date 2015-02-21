using scheduler.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler.data
{
    public interface IRepository
    {
        List<IAssignment> Assignments { get; set; }
        List<IEmployee> Employees { get; set; }
 
    }
}
