using System;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpPost]
        public ActionResult Upload()
        {
            Console.WriteLine("In upload!!");
            return Json(new {});
        }
    }
}