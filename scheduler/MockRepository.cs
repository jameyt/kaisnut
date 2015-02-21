using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.data;

namespace scheduler
{
    public class MockRepository : IRepository
    {
        public List<IAssignment> Assignments { get; set; }

        public List<IEmployee> Employees { get; set; }

        private MockRepository() { }

        public static MockRepository Create()
        {
            var mockRepo = new MockRepository();

            mockRepo.Seed();

            return mockRepo;
        }

        private void Seed()
        {
            Assignments = new List<IAssignment>
            {
                Assignment.Create(
                Role.First, 
                Employee.Create(),
                new DateTime(2015,3,1)
                )
            };
            
            Employees = new List<IEmployee>();
        }

    }
}
