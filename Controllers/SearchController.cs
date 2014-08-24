using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Filters;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using MyWebApp.SearchLucene;
using MyWebApp.ViewModels;

namespace MyWebApp.Controllers
{
    [Culture]
    public class SearchController : Controller
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

        private IProblemRepository problemRepository;

        public SearchController(IProblemRepository problemRepository)
        {
            this.problemRepository = problemRepository;

        }

        [HttpPost]
        public ActionResult Search(string searchingString)
        {
           
            if (string.IsNullOrWhiteSpace(searchingString))
            {
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            var searchUsingLucene = new LuceneSearch(problemRepository,UserManager);
            var foundIds = searchUsingLucene.SearchResult(searchingString);
            var searchModel = new SearchResultModel(){Problems = new List<Problem>(),Users = new List<ApplicationUser>()};
            foreach (var IdFoundElement in foundIds)
            {
                searchModel.Problems.Add(problemRepository.GetByID(IdFoundElement));
            }
            var searchUsers = new SearchUserLucene(UserManager);
            var foundUserIds = searchUsers.SearchResult(searchingString);
            foreach (var IdFoundElement in foundUserIds)
            {
                searchModel.Users.Add(UserManager.FindById(IdFoundElement));
            }
            return View(searchModel);
        }
    }
}