using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Contact
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        private Contact(){}

        public static Contact Create()
        {
            return new Contact();
        }

    }
}
