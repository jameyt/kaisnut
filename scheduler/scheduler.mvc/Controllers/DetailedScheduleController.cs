using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scheduler.data;

namespace scheduler.mvc.Controllers
{
    public class DetailedScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index(int? month, int? day, int? year)
        {
            DateTime date;
            if (!month.HasValue || !day.HasValue || year.HasValue){date = DateTime.Now;}
            else{date = new DateTime(year.Value,month.Value,day.Value);}
            
            var repo = Repository.Create();

            var detailedSchedule = repo.GetDetailedSchedule(date);

            return View(detailedSchedule);
        }

        public ActionResult Edit(int? month, int? day, int? year)
        {
            DateTime date;
            if (!month.HasValue || !day.HasValue || year.HasValue) { date = DateTime.Now; }
            else { date = new DateTime(year.Value, month.Value, day.Value); }

            var repo = Repository.Create();

            var detailedSchedule = repo.GetDetailedSchedule(date);

            return View(detailedSchedule);
        }

    }
}