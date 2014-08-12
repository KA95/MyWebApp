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
        private GenericRepository<Problem> problemRepository = new GenericRepository<Problem>();
        private GenericRepository<ApplicationUser> userRepository = new GenericRepository<ApplicationUser>();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Problems()
        {
            return View(problemRepository.Get());
        }
        public ActionResult Users()
        {
            return View(userRepository.Get());
        }

        [HttpGet]
        public ActionResult DeleteProblem(int? id)
        {
            if (id != null)
            {
                problemRepository.Delete(id);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            if (id != null)
            {
                userRepository.Delete(id);
            }
            return RedirectToAction("Index");
        }


    }
}