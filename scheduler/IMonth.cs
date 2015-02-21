using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public interface IMonth
    {
       DateTime Date { get; set; }
       List<IDay> Days { get; set; }

       void AddAssignment(DateTime date, IAssignment assignment);
       List<IAssignment> GetAssignments();
       List<IAssignment> GetAssignments(IEmployee employee);
       List<IAssignment> GetAssignments(DateTime date);
       List<IAssignment> GetAssignments(IEmployee employee, DateTime date);
       
    }
}
