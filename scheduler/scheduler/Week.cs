using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Week:IWeek
    {
       public List<IDay> Days { get; set; }
    }
}
