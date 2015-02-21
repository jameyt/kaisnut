using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Contact Contact { get; set; }
        
        public double VacationDaysRemaining { get; set; }

        public List<Vacation> VacationScheduled { get; set; }
        public List<Vacation> VacationRequested { get; set; }




    }
}
