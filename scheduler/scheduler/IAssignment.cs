using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IAssignment
    {
        Role Role { get; set; }
        IEmployee Employee { get; set; }
        DateTime Date { get; set; }
        string ShortDate { get; }
        string FormattedRole { get; }
    }
}