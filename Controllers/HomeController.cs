using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWebApp.Filters;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly ITagRepository tagRepository;

        public HomeController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        public ActionResult Index()
        {
            return View( tagRepository.Get().OrderBy(m=>m.Problems.Count));
        }
        public ActionResult Markdown()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SetTheme(string theme="superhero")
        {
            var themes= new List<string>{"yeti","superhero"};
            if (!themes.Contains(theme))
            {
                theme = "superhero";
            }
            HttpCookie cookie = Request.Cookies["theme"];

            if (cookie != null)
            {
                cookie.Value = theme;
            }
            else
            {
                cookie = new HttpCookie("theme");
                cookie.Value = theme;
                cookie.HttpOnly = false;
                cookie.Expires = DateTime.Now.AddDays(100);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }
     
        public ActionResult ChangeCulture(string lang)
        {
            string urlToReturn = Request.UrlReferrer.AbsolutePath;
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie == null)
                cookie =AddLanguageToCookie(lang);
            else
                cookie.Value = lang;
            this.Response.Cookies.Add(cookie);
            return Redirect(urlToReturn);
        }

        private HttpCookie AddLanguageToCookie(string localizationValue)
        {
            var cookie = new HttpCookie("lang");
            cookie.HttpOnly = false;
            cookie.Value = localizationValue;
            cookie.Expires = DateTime.Now.AddYears(1);
            return cookie;
        }

        public JsonResult GetTagStrings()
        {
            IEnumerable<Tag> tags = tagRepository.Get();

            IEnumerable<string> tagStrings = from tag in tags
                select tag.Name;

            return Json(tagStrings.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}