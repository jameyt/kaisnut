using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Employee :IEmployee
    {
        public string First { get; set; }
        public string Last { get; set; }

        public DateTime Start { get; set; }

        public double YearsService
        {
            get
            {
                var time = DateTime.Now - Start;
                return time.Days / 365.25;
            }
        }

        public IContact Contact { get; set; }

        public double VacationDaysRemaining { get; set; }

        public List<IVacation> VacationScheduled { get; set; }
        public List<IVacation> VacationRequested { get; set; }

        public bool Equals(Employee other)
        {
            return other != null
                && other.First == First
                && other.Last == Last
                && other.Contact.Phone == Contact.Phone;
        }

        private Employee() { }

        public static Employee Create(string firstName, string lastName, DateTime start, string phone, string email, string address)
        {
            var employee = new Employee
            {
                First = firstName,
                Last = lastName,
                Start = start,
                Contact = scheduler.Contact.Create(
                                                        phone,
                                                        email,
                                                        address)
            };
            return employee;
        }

        public static Employee Create(string firstName, string lastName, DateTime start, Contact contact)
        {
            var employee = new Employee
            {
                First = firstName,
                Last = lastName,
                Start = start,
                Contact = contact
            };
            return employee;
        }


        public static Employee CreateEmpty()
        {
            return new Employee
            {
                Contact = scheduler.Contact.CreateEmpty(),
                First = string.Empty,
                Last = string.Empty,
                Start = DateTime.MinValue
            };
        }
    }
}
