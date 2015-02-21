using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.core;

namespace scheduler.data
{
    public class LocalRepository:IRepository
    {
        private LocalRepository() { }

        public static LocalRepository Create()
        {
            return new LocalRepository();
        }
        
        public List<IAssignment> Assignments
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

        public List<IEmployee> Employees
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
    }
}
