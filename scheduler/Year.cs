using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Year
    {
        public DateTime Date { get; private set; }
        public List<Month> Months { get; private set; } 

        private Year()
        {
            
        }

        public static Year Create(DateTime year)
        {
            return new Year(){Date = year};
        }
    }
}
