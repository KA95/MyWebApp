using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Show(string id)
        {
            return View();
        }
    }
}