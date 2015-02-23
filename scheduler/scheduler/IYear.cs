using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public interface IYear
    {
       DateTime Date { get; set; }
       List<IMonth> Months { get; }
       List<IAssignment> GetAssignments(IEmployee employee);
       List<IAssignment> GetAssignments(DateTime date);
       List<IAssignment> GetAssignments(IEmployee employee, DateTime date);
       void AddAssignment(IAssignment assignment);
    }
}
