using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Assignment
    {
        public Role Role { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }

        private Assignment(){}

        public static Assignment Create(Role role, Employee employee, DateTime date)
        {
            return new Assignment
            {
                Role = role, 
                Employee = employee, 
                Date = date
            };
        }

        public static Assignment CreateEmpty() { return new Assignment();}
    }
}
