using MyWebApp.Models;
using MyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MyWebApp.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private GenericRepository<Problem> problemRepository = new GenericRepository<Problem>();
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
            return View(UserManager.Users);
        }

        [HttpGet]
        public ActionResult DeleteProblem(int? id)
        {
            if (id != null)
            {
                problemRepository.Delete(id);
            }
            return RedirectToAction("Problems");
        }
        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            if (id != null)
            {
                UserManager.Delete(UserManager.FindById(id));
            }
            return RedirectToAction("Users");
        }


    }
}