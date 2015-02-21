using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler.core
{
   public interface IDataAccessLayer
    {
       IDbConnection Connection { get; set; }

       void SaveEmployees(List<IEmployee> value);

       List<IEmployee> GetEmployees();

       void SaveAssignments(List<IAssignment> value);

       List<IAssignment> GetAssignments();

    }
}
