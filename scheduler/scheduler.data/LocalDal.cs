using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.core;

namespace scheduler.data
{
    public class LocalDal:IDataAccessLayer
    {
        public System.Data.IDbConnection Connection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        
        public void SaveEmployees(List<IEmployee> value)
        {
            throw new NotImplementedException();
        }

        public List<IEmployee> GetEmployees()
        {
            throw new NotImplementedException();
        }

        public void SaveAssignments(List<IAssignment> value)
        {
            throw new NotImplementedException();
        }

        public List<IAssignment> GetAssignments()
        {
            throw new NotImplementedException();
        }
    }
}
