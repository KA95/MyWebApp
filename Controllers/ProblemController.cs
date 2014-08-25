using System.IO;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using MyWebApp.Helpers;
using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Repositories.Interfaces;
using System.Collections.ObjectModel;
using MarkdownDeep;

namespace MyWebApp.Controllers
{

    public class ProblemController : Controller
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

        public ProblemController(IProblemRepository problemRepository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository, IImageRepository imageRepository, ITagRepository tagRepository, ILikeRepository likeRepository, IDislikeRepository dislikeRepository, ICommentRepository commentRepository)
        {
            this.problemRepository = problemRepository;
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

        public ActionResult Index()
        {
            return View(problemRepository.Get());
        }

        [HttpGet]
        public ActionResult Show(int ?id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Problem problem = problemRepository.GetByID(id);

            var md = new Markdown();
            problem.Text = md.Transform(problem.Text);

            IEnumerable<UserSolvedProblem> usersSolved = userSolvedRepository.Get(m => m.ProblemId == problem.Id);
            IEnumerable<UserSolvedProblem> userProblem =
                from userSolution in usersSolved
                where userSolution.User.UserName == User.Identity.Name
                select userSolution;

            //usersSolved.Contains()
            var pvm = GlobalHelper.GetShowProblemViewModel(problem);
            pvm.IsSolved = userProblem.Count() != 0;
            pvm.Answers = "";
            return View(pvm);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Show(ShowProblemViewModel problemView)//Actually it's Solve method.
        {
            if (problemView.Author == User.Identity.Name)
            {
                problemView.Answers = "";
                return View(problemView);

            }

            Problem problem = problemRepository.GetByID(problemView.Id);
            var pvm = GlobalHelper.GetShowProblemViewModel(problem);
            pvm.IsSolved = false;
            if (ModelState.IsValidField("Answers"))
                if (GlobalHelper.AreEqual(GetAnswersFromString(problemView.Answers, problem), problem.CorrectAnswers))
                {
                    userSolvedRepository.Insert(new UserSolvedProblem { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problem.Id });
                    var user = UserManager.FindByName(User.Identity.Name);
                    int attempts= (from attempt in user.AttemptedProblems
                                       where attempt.User==user
                                       select attempt).Count();
                    double rating = 100;
                    for (int i = 0; i < attempts; i++)
                        rating *= 0.95;

                    user.Rating += (int)rating;
                    UserManager.Update(user);
                    pvm.IsSolved = true;
                }
                else
                {
                    userAttemptedRepository.Insert(new UserAttemptedProblem { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problem.Id });
                }


            var md = new Markdown();

            problem.Text = md.Transform(problem.Text);

            return View(pvm);
        }

        public ActionResult Tutorial()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Button = "Create";
            return View(new EditProblemViewModel { TagsString = "" });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(EditProblemViewModel problemView)
        {
            if (ModelState.IsValidField("Answers"))
                AddProblemFromView(problemView);
            else
                return View(problemView);

            problemView.Author = User.Identity.Name;
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            
            ViewBag.Button = "Edit";
            Problem problem = problemRepository.GetByID(id);

            if (problem.Author.UserName != User.Identity.Name && !User.IsInRole("admin"))
            {
                return RedirectToAction("Index");
            }

            var pvm = GlobalHelper.GetEditProblemViewModel(problem);
      
            return View(pvm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditProblemViewModel problemView)
        {
            if (ModelState.IsValidField("Answers"))
                UpdateProblemFromView(problemView);
            else
                return View(problemView);

            problemView.Author = User.Identity.Name;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public JsonResult Like(int problemId)
        {
            Problem problem = problemRepository.GetByID(problemId);

            
            var likes = problem.Likes.Count;
            var dislikes = problem.Dislikes.Count;

            if (problem.Author.UserName == User.Identity.Name)
                return Json(new { l = likes, d = dislikes });

            bool haslike = likeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;
            bool hasdislike = dislikeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;

            int dislikeId = 0;
            int likeId = 0;

            if (haslike) likeId = likeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;
            if (hasdislike) dislikeId = dislikeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;

            if (haslike)
            {
                likeRepository.Delete(likeId);
                likes--;
            }
            else
            {
                if (hasdislike)
                {
                    likeRepository.Insert(new Like { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    dislikeRepository.Delete(dislikeId);
                    dislikes--;
                    likes++;
                }
                else
                {
                    likeRepository.Insert(new Like { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    likes++;
                }
            }

            return Json(new { l = likes, d = dislikes });

        }

        [HttpPost]
        [Authorize]
        public JsonResult Dislike(int problemId)
        {
            Problem problem = problemRepository.GetByID(problemId);
            var likes = problem.Likes.Count;
            var dislikes = problem.Dislikes.Count;

            if (problem.Author.UserName == User.Identity.Name)
                return Json(new { l = likes, d = dislikes });

            bool haslike = likeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;
            bool hasdislike = dislikeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;

            int dislikeId = 0;
            int likeId = 0;

            if (haslike) likeId = likeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;
            if (hasdislike) dislikeId = dislikeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;

            if (hasdislike)
            {
                dislikeRepository.Delete(dislikeId);
                dislikes--;
            }
            else
            {
                if (haslike)
                {
                    dislikeRepository.Insert(new Dislike { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    likeRepository.Delete(likeId);
                    dislikes++;
                    likes--;
                }
                else
                {
                    dislikeRepository.Insert(new Dislike { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    dislikes++;
                }
            }

            return Json(new { l = likes, d = dislikes });

        }

        [HttpPost]
      
        public ActionResult AddComment(int problemId, string commentText)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(commentText))
                return Json(new { commentHtml = "" });

            commentRepository.Insert(new Comment { dateTime = DateTime.Now, ProblemId = problemId, Text = commentText, UserId = UserManager.FindByName(User.Identity.Name).Id });

            var cvm = new CommentViewModel()
            {
                AddingTime = DateTime.Now,
                AuthorName = User.Identity.Name,
                ProblemId = problemId,
                Text = commentText
            };

            return Json(new { html = RenderPartialViewToString("_CommentView", cvm) });
        }


        [HttpPost]
        [Authorize]
        public ActionResult UploadFile(HttpPostedFileBase myFile)
        {
            if (myFile == null)
            {
                return Json("");
            }
            var cloudinary = new Cloudinary(
            new Account(
            "dbyvro3qo",
            "649126626476462",
            "CZ_W5wnwG_yxvpHfvJpaepZf9n8"));

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(myFile.FileName, myFile.InputStream),
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            var uplPath = uploadResult.Uri;

            return Json(uplPath);
        }

        #region helpers

        public void AddProblemFromView(EditProblemViewModel problemView)
        {


            var problem = new Problem
            {
                CategoryId = problemView.SelectedCategoryId,
                Name = problemView.Name,
                Text = problemView.Text,
                AuthorId = UserManager.FindByName(User.Identity.Name).Id,
                Tags = new Collection<Tag>(),
                IsBlocked = problemView.IsBlocked
            };

            foreach (var tag in problemView.TagsString.Split(','))
            {
                problem.Tags.Add(tagRepository.GetByName(tag));
            }

            problemRepository.Insert(problem);
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            problemRepository.Update(problem);
        }

        public void UpdateProblemFromView(EditProblemViewModel problemView)
        {
            Problem problem = problemRepository.GetByID(problemView.Id);
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            problem.IsBlocked = problemView.IsBlocked;
            while (problem.CorrectAnswers.Count != 0)
                answerRepository.Delete(problem.CorrectAnswers.ElementAt(0));
    
       
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            problem.Tags = new Collection<Tag>();
            problemRepository.Update(problem);
            if (!problemView.TagsString.IsNullOrWhiteSpace())
            {
                foreach (var tag in problemView.TagsString.Split(','))
                {
                    problem.Tags.Add(tagRepository.GetByName(tag));
                }
            }
            problemRepository.Update(problem);
        }

        private ICollection<Answer> GetAnswersFromString(string answers, Problem problem)
        {
            string[] answersArray = answers.Split(';');

            var collection = new Collection<Answer>();
            foreach (var ans in answersArray)
            {

                collection.Add(new Answer() { Text = ans.Trim(), Problem = problem });
            }
            return collection;
        }

        //[HttpPost]
        //public ActionResult InfiniteScroll(int? taskId, int blockNumber)
        //{
        //    System.Threading.Thread.Sleep(1000);
        //    const int blockSize = 5;
        //    var comments = GetTasksComments(taskId, blockNumber);
        //    var jsonModel = new JsonModel
        //    {
        //        NoMoreData = comments.Count < blockSize,
        //        HtmlString = RenderPartialViewToString("_CommentViewPartial", comments),
        //    };
        //    return Json(jsonModel);
        //}

        private string RenderPartialViewToString(string viewName, CommentViewModel comment)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }
            ViewData.Model = comment;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}