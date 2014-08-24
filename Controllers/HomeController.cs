using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Filters;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.ViewModels;

namespace MyWebApp.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private readonly IProblemRepository problemRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IUserSolvedRepository userSolvedRepository;
        private readonly IUserAttemptedRepository userAttemptedRepository;
        private readonly IImageRepository imageRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IDislikeRepository dislikeRepository;
        private readonly ICommentRepository commentRepository;
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


        public HomeController(IProblemRepository repository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository, IImageRepository imageRepository, ITagRepository tagRepository, ILikeRepository likeRepository, IDislikeRepository dislikeRepository, ICommentRepository commentRepository)
        {
            this.problemRepository = repository;
            this.categoryRepository = categoryRepository;
            this.answerRepository = answerRepository;
            this.userAttemptedRepository = userAttemptedRepository;
            this.userSolvedRepository = userSolvedRepository;
            this.tagRepository = tagRepository;
            this.imageRepository = imageRepository;
            this.likeRepository = likeRepository;
            this.dislikeRepository = dislikeRepository;
            this.commentRepository = commentRepository;
        }

        public ActionResult Index()
        {
            var model = new HomepageViewModel
            {
                Tags = tagRepository.Get().OrderBy(m => m.Problems.Count),
                PopularProblems = GetPopularProblems(),
                RecentProblems = GetRecentProblems(),
                TopUsers = GetTopUsers()
            };
            return View(model);
        }

        private IList<HomepageUser> GetTopUsers()
        {
            IList<ApplicationUser> users = UserManager.Users.ToList();
            users = users.OrderByDescending(m => m.Rating).Take(10 < users.Count() ? 10 : users.Count()).ToList();
            return users.Select(user => new HomepageUser
            {
                Name = user.UserName,
                Rating = user.Rating
            }).ToList();

        }

        private IList<HomepageProblem> GetPopularProblems()
        {
            IList<Problem> problems = problemRepository.Get().ToList();
            problems = problems.OrderByDescending(problem => problem.Likes.Count + problem.Dislikes.Count + problem.UsersWhoAttempted.Count + problem.UsersWhoSolved.Count).Take(10 < problems.Count() ? 10 : problems.Count()).ToList();
            return problems.Select(problem => new HomepageProblem
            {
                Name = problem.Name,
                Category = problem.Category.Name,
                Rating = problem.Likes.Count + problem.Dislikes.Count + problem.UsersWhoAttempted.Count + problem.UsersWhoSolved.Count
            }).ToList();
        }

        private IList<HomepageProblem> GetRecentProblems()
        {
            IList<Problem> problems = problemRepository.Get().ToList();
            problems = problems.OrderByDescending(m => m.Id).Take(10 < problems.Count() ? 10 : problems.Count()).ToList();                          
            return problems.Select(problem => new HomepageProblem
            {
                Name = problem.Name, Category = problem.Category.Name, Rating = problem.Likes.Count + problem.Dislikes.Count + problem.UsersWhoAttempted.Count + problem.UsersWhoSolved.Count
            }).ToList();

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