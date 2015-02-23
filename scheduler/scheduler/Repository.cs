using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler.data
{
    public class Repository:IRepository
    {
        private IDbConnection Connection { get; set; }

        private Repository() { }

        public static Repository Create(IDbConnection cn)
        {
            return new Repository(){Connection = cn};
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
