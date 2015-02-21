using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Employee : IEquatable<Employee>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Contact Contact { get; set; }

        public double VacationDaysRemaining { get; set; }

        public List<Vacation> VacationScheduled { get; set; }
        public List<Vacation> VacationRequested { get; set; }

        public bool Equals(Employee other)
        {
            return other != null 
                && other.FirstName == FirstName 
                && other.LastName == LastName 
                && other.Contact.Phone == Contact.Phone;
        }
    }
}
