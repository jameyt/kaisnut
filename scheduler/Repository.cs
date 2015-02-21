using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.core;

namespace scheduler.data
{
    public class Repository:IRepository
    {
        private Repository() { }

        public static Repository Create()
        {
            return new Repository(){};
        }

        public List<IAssignment> Assignments
        {
            get { return GetAssignments(); }
            set { SaveAssignments(value); }
        }

        public List<IEmployee> Employees
        {
            get { return GetEmployees(); }
            set { SaveEmployees(value); }
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
