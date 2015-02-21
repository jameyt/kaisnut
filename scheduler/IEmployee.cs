using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IEmployee : IEquatable<Employee>
    {
      string First { get; set; }
      string Last { get; set; }

      DateTime Start { get; set; }

      double YearsService { get; }

      IContact Contact { get; set; }

      double VacationDaysRemaining { get; set; }

      List<IVacation> VacationScheduled { get; set; }
      List<IVacation> VacationRequested { get; set; }


    }
}
