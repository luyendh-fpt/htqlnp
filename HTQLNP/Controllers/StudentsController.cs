using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HTQLNP.Models;
using PagedList;

namespace HTQLNP.Controllers
{
    public class StudentsController : Controller
    {
        private MyDBContext db = new MyDBContext();

        public ActionResult GetListStudentData()
        {
            return new JsonResult()
            {
                Data = db.Students.Where(s => s.Status != StudentStatus.Deleted).ToList(), 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: Students
        public ActionResult Index(string sortOrder, string keyword, int? page, int? limit)
        {
            // sortOrder sắp xếp theo cái gì.
            // name_desc
            // date_desc
            var students = db.Students.Where(s => s.Status != StudentStatus.Deleted);

            if (!string.IsNullOrEmpty(keyword))
            {
                students = students.Where(s => s.FullName.Contains(keyword) || s.Email.Contains(keyword));
                ViewBag.CurrentFilter = keyword;
            }

            students = students.OrderByDescending(s => s.CreatedAt);

            var nameOrder = "name_asc";
            var dateOrder = "date_asc";
            if (string.IsNullOrEmpty(sortOrder)
                || sortOrder.Equals("date_asc")
                || sortOrder.Equals("name_asc"))
            {
                nameOrder = "name_desc";
                dateOrder = "date_desc";
            }
            ViewBag.NameSortParameter = nameOrder;
            ViewBag.DateSortParameter = dateOrder;
            ViewBag.CurrentSort = sortOrder;

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.FullName);
                    break;
                case "name_asc":
                    students = students.OrderBy(s => s.FullName);
                    break;
                case "date_asc":
                    students = students.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.CreatedAt);
                    break;
            }

            int _limit = (limit ?? 10);
            int _page = (page ?? 1);

            ViewBag.CurrentPage = _page;
            ViewBag.Limit = _limit;
            ViewBag.TotalPage = Math.Ceiling((double) students.Count() / _limit);
                
            //return View(students.ToPagedList(pageNumber, pageSize));
            return View(students.Skip((_page - 1) * _limit).Take(_limit).ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student == null || student.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RollNumber,FullName,Email,CreatedAt,UpdatedAt,DeletedAt,Status")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.CreatedAt = DateTime.Now;
                student.UpdatedAt = DateTime.Now;
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var student = db.Students.Find(id);
            if (student == null || student.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RollNumber,FullName,Email,Status")] Student student)
        {
            if (student == null ||student.RollNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existStudent = db.Students.Find(student.RollNumber);
            if (existStudent == null || existStudent.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (ModelState.IsValid)
            {
                existStudent.FullName = student.FullName;
                existStudent.Email = student.Email;
                existStudent.Status = student.Status;
                db.Students.AddOrUpdate(existStudent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null || student.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existStudent = db.Students.Find(id);
            if (existStudent == null || existStudent.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (ModelState.IsValid)
            {
                existStudent.Status = StudentStatus.Deleted;
                existStudent.DeletedAt = DateTime.Now;
                db.Students.AddOrUpdate(existStudent);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
