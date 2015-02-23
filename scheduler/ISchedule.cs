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
        List<IYear> Years { get; set; }
        List<IEmployee> Employees { get; set; }

        void AddAssignment(IAssignment assignment);

        List<IAssignment> GetAssignment(DateTime date);
        void InitializeCurrentYear();
        void InitializeThreeYear();
    }
}
