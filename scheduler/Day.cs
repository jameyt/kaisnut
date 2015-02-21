﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public class Day
    {
      public List<Role> Roles { get; private set; }

      public List<Assignment> Assignments { get; private set; }

      public List<Employee> Employees { get; private set; }

      public DateTime Date { get; private set; }

       private Day(){}

       public static Day Create(List<Assignment> assignments, DateTime date)
       {
           if (assignments == null) { assignments = new List<Assignment>();}
           return new Day()
           {
               Roles = (from assignment in assignments select assignment.Role).ToList(),
               Employees = (from assignment in assignments select assignment.Employee).ToList(),
               Assignments = assignments, 
               Date = date,
           };
       }

       public List<Assignment> GetAssignmentsByEmployee(Employee employee   )
       {
           return (from assignment in Assignments 
                  where employee.Equals(assignment.Employee) 
                  select assignment).ToList();
       }

    }
}
