using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Filters;
using MyWebApp.Repositories.Interfaces;
//
namespace MyWebApp.Controllers
{
    [Culture]
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IProblemRepository problemRepository;
        public AdminController(IProblemRepository problemRepository)
        {
            this.problemRepository = problemRepository;
        }
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
        public ActionResult LockUser(string id)
        {
            if (User.Identity.Name == UserManager.FindById(id).UserName)
                return RedirectToAction("Users");

            UserManager.FindById(id).IsBlocked = true;

            return RedirectToAction("Users");
        }
        [HttpGet]
        public ActionResult UnlockUser(string id)
        {
            if (User.Identity.Name == UserManager.FindById(id).UserName)
                return RedirectToAction("Users");

            UserManager.FindById(id).IsBlocked = false;
          
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            if (id != null)
            {
            }
            return RedirectToAction("Users");
        }

    }
}