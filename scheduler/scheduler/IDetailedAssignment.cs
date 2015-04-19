using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IDetailedAssignment
    {
        int Id { get; set; }
        string Location { get; set; }
        string Surgeon { get; set; }
        string Time { get; set; }
        string Provider { get; set; }
        string PeelOut { get; set; }
        Hospital Hospital { get; set; }
        DateTime Date { get; set; }
    }
}
