using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler.core;
using scheduler.data;

namespace scheduler.tests
{
    public class BindingService
    {
        public static IRepository GetRepository()
        {
            var repo =  Repository.Create();
        }
    }
}
