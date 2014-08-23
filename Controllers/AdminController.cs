using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Repositories.Interfaces;
//
namespace MyWebApp.Controllers
{
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