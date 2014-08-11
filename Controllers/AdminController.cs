using MyWebApp.Models;
using MyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private GenericRepository<Problem> repository = new GenericRepository<Problem>();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Problems()
        {
            return View(repository.Get());
        }
        public ActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                repository.Delete(id);
            }
            return RedirectToAction("Index");
        }

    }
}