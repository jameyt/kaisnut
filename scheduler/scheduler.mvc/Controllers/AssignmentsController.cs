using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AAON.Utility.Objects.Methods;
using scheduler.data;

namespace scheduler.mvc.Controllers
{
    public class AssignmentsController : Controller
    {
        // GET: Assignments
        public ActionResult Index(int? month, Role? role, string first,string last)
        {
            var repo = Repository.Create();
            var assignments = repo.Assignments;

            if (month != null)
            {
                assignments = assignments.Where(assignment => assignment.Date.Month == month.Value).ToList();
            }
            if (role != null && role != Role.Any)
            {
                assignments = assignments.Where(assignment => assignment.Role == role.Value).ToList();
            }
            if (!string.IsNullOrWhiteSpace(first))
            {
                assignments = assignments.Where(assignment => assignment.Employee.First == first).ToList();
            }
            if (!string.IsNullOrWhiteSpace(last))
            {
                assignments = assignments.Where(assignment => assignment.Employee.Last == last).ToList();
            }

            return View(assignments);
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null){id = 0;}
            var repo = Repository.Create();

            return View(repo.Assignments[id.Value]);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Assignments/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var repo = Repository.Create();
                var schedule = Schedule.Create(repo);
               
                var role = (Role)Enum.Parse(typeof(Role), collection["Role"]);
                var date = Parsers.Parser(collection["Date"], DateTime.MinValue);
                var employeeInitials = Parsers.Parser(collection["Employee.Initials"],"");
                var employee = repo.GetEmployeeByInitials(employeeInitials);

                if (employee == null )
                {
                    ViewBag.ErrorMessage = "Employee not found for given initials.";
                    return View();
                }

                if ((from a in schedule.GetAssignment(date)
                    where a.Employee == employee
                    select a).Any())
                {
                    ViewBag.ErrorMessage = "Assignment already exists for employee on this date.";
                    return View();
                }
                
                var assignment = Assignment.Create(role, employee, date);
                repo.AddAssignment(assignment);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) { return RedirectToAction("Index"); }

            var repo = Repository.Create();
            
            var assignment = (from a in repo.Assignments where id.Value == a.Id select a).FirstOrDefault();
            if (assignment == null) { return RedirectToAction("Index"); }
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var repo = Repository.Create();
                
                var role = (Role)Enum.Parse(typeof(Role), collection["Role"]);
                var date = Parsers.Parser(collection["Date"], DateTime.MinValue);
                var employeeInitials = Parsers.Parser(collection["Employee.Initials"], "");
                var employee = repo.GetEmployeeByInitials(employeeInitials);

                if (employee == null)
                {
                    ViewBag.ErrorMessage = "Employee not found for given initials.";
                    return View();
                }
                
                var assignment = repo.GetAssignment(id);
                assignment.Role = role;
                assignment.Date = date;
                assignment.Employee = employee;
                repo.UpdateAssignment(assignment);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) { return RedirectToAction("Index"); }
            var repo = Repository.Create();
            var assignment = (from a in repo.Assignments where id.Value == a.Id select a).FirstOrDefault();
            if (assignment == null) { return RedirectToAction("Index"); }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var repo = Repository.Create();
                repo.DeleteAssignment(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
