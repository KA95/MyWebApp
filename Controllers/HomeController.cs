using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult SetLanguage(string language = "english")
        {
            var languages = new List<string> { "русский", "english" };
            if (!languages.Contains(language))
            {
                language = "english";
            }
            HttpCookie cookie = Request.Cookies["language"];

            if (cookie != null)
            {
                cookie.Value = language;
            }
            else
            {
                cookie = new HttpCookie("language");
                cookie.Value = language;
                cookie.HttpOnly = false;
                cookie.Expires = DateTime.Now.AddDays(100);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}