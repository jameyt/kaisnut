using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace scheduler.mvc.Controllers
{
    public class AssignmentsController : Controller
    {
        // GET: Assignments
        public ActionResult Index(int? month, Role? role, string first,string last)
        {
            var mockRepo = MockRepository.Create();
            var assignments = mockRepo.Assignments;

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
            var mockRepo = MockRepository.Create();

            return View(mockRepo.Assignments[id.Value]);
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
                // TODO: Add insert logic here

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
            if (id == null) { id = 0; }

            var mockRepo = MockRepository.Create();

            return View(mockRepo.Assignments[id.Value]);
        }

        // POST: Assignments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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
            if (id == null) { id = 0; }

            var mockRepo = MockRepository.Create();

            return View(mockRepo.Assignments[id.Value]);
        }

        // POST: Assignments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
