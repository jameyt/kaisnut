using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace scheduler.mvc.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Month(int id)
        {
            var mockRepo = MockRepository.Create();
            var schedule = Schedule.Create(mockRepo);

            schedule.Years[1].Months[id].SetName(id);
            return View(schedule.Years[1].Months[id]);
        }
    }
}