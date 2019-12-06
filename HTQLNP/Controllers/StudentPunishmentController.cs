using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTQLNP.Models;

namespace HTQLNP.Controllers
{
    
    public class StudentPunishmentController : Controller
    {
        private static List<StudentPunishment> list = new List<StudentPunishment>();
        // GET: StudentPunishment
        public ActionResult Index()
        {
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(StudentPunishment studentPunishment)
        {
            studentPunishment.CreatedAt = DateTime.Now;
            list.Add(studentPunishment);
            TempData["msg"] = "Action Success!";
            return Redirect("/StudentPunishment/Index");

        }
    }
}