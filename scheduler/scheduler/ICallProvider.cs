using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface ICallProvider
    {
        int Id { get; set; }
        string Provider { get; set; }
        string Name { get; set; }
        string Notes { get; set; }
        string InTime { get; set; }
        Hospital Hospital { get; set; }
        DateTime Date { get; set; }
    }
}
