using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public interface IVacation
    {
       DateTime Start { get; set; }
       DateTime End { get; set; }

       TimeSpan Length { get; }

       double Days { get; }

    }
}
