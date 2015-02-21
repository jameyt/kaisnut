using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface ISchedule
    {

        List<Year> Years { get; set; }

        void AddAssignment(Assignment assignment);

    }
}
