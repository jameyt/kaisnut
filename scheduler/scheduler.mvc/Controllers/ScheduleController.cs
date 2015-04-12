using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scheduler.data;

namespace scheduler.mvc.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index()
        {
            var repo = Repository.Create();
            var schedule = Schedule.Create(repo);

            for (var i = 0; i < schedule.Years[1].Months.Count; i++)
            {
                var month = schedule.Years[1].Months[i];
                month.SetName(i);
            }
            return View(schedule.Years[1].Months);
        }

        public ActionResult Details(int? id, string name)
        {
            if (id != null)
            {
                id--;
                var repo = Repository.Create();
                var schedule = Schedule.Create(repo);

                schedule.Years[1].Months[id.Value].SetName(id.Value);
                return View(schedule.Years[1].Months[id.Value]);
            }
            else if (name != null)
            {
                var repo = Repository.Create();
                var schedule = Schedule.Create(repo);

                for (var i = 0; i < schedule.Years[1].Months.Count; i++)
                {
                    var month = schedule.Years[1].Months[i];
                    month.SetName(i);
                }

                foreach (var month in schedule.Years[1].Months)
                {
                    if (month.Name == name)
                    {
                        return View(month);
                    }
                }

            }
            return View();
        }
    }
}