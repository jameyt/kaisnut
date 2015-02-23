using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public class Day:IDay
    {
      public List<Role> Roles { get; private set; }

      public List<IAssignment> Assignments { get; private set; }

      public List<IEmployee> Employees { get; private set; }

      public DateTime Date { get; private set; }

       private Day(){}

       public static Day Create(List<IAssignment> assignments, DateTime date)
       {
           if (assignments == null) { assignments = new List<IAssignment>();}
           return new Day()
           {
               Roles = (from assignment in assignments select assignment.Role).ToList(),
               Employees = (from assignment in assignments select assignment.Employee).ToList(),
               Assignments = assignments, 
               Date = date,
           };
       }

       public static Day CreateEmpty()
       {
           return new Day();
       }

       public List<IAssignment> GetAssignmentsByEmployee(IEmployee employee   )
       {
           return (from assignment in Assignments 
                  where employee.Equals(assignment.Employee) 
                  select assignment).ToList();
       }

    }
}
