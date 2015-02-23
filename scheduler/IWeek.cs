using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public interface IWeek
    {
       List<IDay> Days { get; set; }
    }
}
