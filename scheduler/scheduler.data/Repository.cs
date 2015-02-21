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
        private IDataAccessLayer Dal { get; set; }

        private Repository() { }

        public static Repository Create()
        {
            return new Repository(){Dal = dal};
        }

        public List<IAssignment> Assignments
        {
            get { return Dal.GetAssignments(); }
            set { Dal.SaveAssignments(value); }
        }

        public List<IEmployee> Employees
        {
            get { return Dal.GetEmployees(); }
            set { Dal.SaveEmployees(value); }
        }

    }
}
