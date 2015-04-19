using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class DetailedSchedule:IDetailedSchedule
    {
        private DetailedSchedule() { }

        public static DetailedSchedule Create() { return new DetailedSchedule();}

        public DateTime Date { get; set; }

        public List<IDetailedAssignment> DetailedAssignments { get; set; }
        
        public List<ICallProvider> CallProviders { get; set; }
    }
}
