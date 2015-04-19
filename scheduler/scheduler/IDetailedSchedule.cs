using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public interface IDetailedSchedule
    {
        DateTime Date { get; set; }
        List<IDetailedAssignment> DetailedAssignments { get; set; }
        List<ICallProvider> CallProviders { get; set; }
    }

    

   

    
}
