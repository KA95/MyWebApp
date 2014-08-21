using System.IO;
using System.Web.Helpers;
using MyWebApp.Models;
using MyWebApp.ViewModels;
using MyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Repositories.Interfaces;
using System.Collections.ObjectModel;
using MarkdownDeep;
using System.Text;

namespace MyWebApp.Controllers
{

    public class ProblemController : Controller
    {

        private readonly IProblemRepository repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IUserSolvedRepository userSolvedRepository;
        private readonly IUserAttemptedRepository userAttemptedRepository;
        private readonly IImageRepository imageRepository;
        private readonly IVideoRepository videoRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IDislikeRepository dislikeRepository;
        private readonly ICommentRepository commentRepository;

        public ProblemController(IProblemRepository repository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository, IImageRepository imageRepository, IVideoRepository videoRepository, ITagRepository tagRepository, ILikeRepository likeRepository, IDislikeRepository dislikeRepository, ICommentRepository commentRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.answerRepository = answerRepository;
            this.userAttemptedRepository = userAttemptedRepository;
            this.userSolvedRepository = userSolvedRepository;
            this.tagRepository = tagRepository;
            this.imageRepository = imageRepository;
            this.videoRepository = videoRepository;
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
            return View(repository.Get());
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            Problem problem = repository.GetByID(id);

            Markdown md = new Markdown();
            problem.Text = md.Transform(problem.Text);

            IEnumerable<UserSolvedProblem> usersSolved = userSolvedRepository.Get(m => m.ProblemId == problem.Id);
            IEnumerable<UserSolvedProblem> userProblem =
                from userSolution in usersSolved
                where userSolution.User.UserName == User.Identity.Name
                select userSolution;

            //usersSolved.Contains()
            var pvm = GetProblemViewModel(problem);
            pvm.Solved = userProblem.Count() != 0;
            pvm.Answers = "";
            return View(pvm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Show(ProblemViewModel problemView)
        {
            int a = 0;
            Problem problem = repository.GetByID(problemView.Id);
            var pvm = GetProblemViewModel(problem);
            pvm.Solved = false;
            if (ModelState.IsValidField("Answers"))
                if (AreEqual(GetAnswersFromString(problemView.Answers, problem), problem.CorrectAnswers))
                {
                    userSolvedRepository.Insert(new UserSolvedProblem() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problem.Id });
                    pvm.Solved = true;
                }
                else
                {
                    userAttemptedRepository.Insert(new UserAttemptedProblem() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problem.Id });
                }


            Markdown md = new Markdown();

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
            Problem problem = new Problem();
            return View(new ProblemViewModel() { Images = new Collection<string>() ,TagsString = ""});
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(ProblemViewModel problemView)
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
            Problem problem = repository.GetByID(id);
            var pvm = GetProblemViewModel(problem);
            if (pvm.Images == null)
                pvm.Images = new Collection<string>();

            return View(pvm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ProblemViewModel problemView)
        {
            if (problemView.Images == null)
                problemView.Images = new Collection<string>();
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
            Problem problem=repository.GetByID(problemId);
            var likes = problem.Likes.Count;
            var dislikes = problem.Dislikes.Count;

            bool haslike=likeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;
            bool hasdislike=dislikeRepository.Get().Where(m => m.ProblemId == problemId).Count(m => m.User.UserName == User.Identity.Name) != 0;

            int dislikeId=0;
            int likeId=0;

            if (haslike) likeId=likeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;
            if (hasdislike) dislikeId=dislikeRepository.Get().Where(m => m.ProblemId == problemId).First(m => m.User.UserName == User.Identity.Name).Id;

            if(haslike)
            {
                likeRepository.Delete(likeId);
                likes--;
            }
            else
            {
                if(hasdislike)
                {          
                    likeRepository.Insert(new Like() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    dislikeRepository.Delete(dislikeId);
                    dislikes--;
                    likes++;
                }
                else
                {
                    likeRepository.Insert(new Like() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    likes++;
                }
            }

            return Json(new { l = likes, d = dislikes });

        }

        [HttpPost]
        [Authorize]
        public JsonResult Dislike(int problemId)
        {
            Problem problem = repository.GetByID(problemId);
            var likes = problem.Likes.Count;
            var dislikes = problem.Dislikes.Count;

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
                    dislikeRepository.Insert(new Dislike() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    likeRepository.Delete(likeId);
                    dislikes++;
                    likes--;
                }
                else
                {
                    dislikeRepository.Insert(new Dislike() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problemId });
                    dislikes++;
                }
            }

            return Json(new { l = likes, d = dislikes });

        }

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(int problemId,string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
                return Json( new {commentHtml = ""});

            commentRepository.Insert(new Comment() { dateTime = DateTime.Now, ProblemId = problemId, Text = commentText, UserId = UserManager.FindByName(User.Identity.Name).Id });

            var cvm = new CommentViewModel()
            {
                AddingTime = DateTime.Now,
                AuthorName = User.Identity.Name,
                ProblemId = problemId,
                Text = commentText
            };

            return Json(new{html = RenderPartialViewToString("_CommentView",cvm)});
        }
        #region helpers

        private ProblemViewModel GetProblemViewModel(Problem problem)
        {
            var pvm = new ProblemViewModel();
            pvm.Author = problem.Author.UserName;
            pvm.Name = problem.Name;
            pvm.Text = problem.Text;
            pvm.Id = problem.Id;
            pvm.SelectedCategoryId = problem.CategoryId;
            pvm.Category = problem.Category.Name;

            StringBuilder sb= new StringBuilder();
            foreach (var tag in problem.Tags)
            {
                sb.Append(tag.Name);
                sb.Append(',');
            }
            
            pvm.TagsString = sb.ToString();

            sb.Clear();

            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }
            sb.Remove(sb.Length - 1, 1);
            pvm.Answers = sb.ToString();

            pvm.Images = new Collection<string>();
            foreach (var image in problem.ImagesInside)
            {
                pvm.Tags.Add(image.URL);
            }

            pvm.Comments = new List<CommentViewModel>();
            foreach (var comment in problem.Comments)
            {
                CommentViewModel com = new CommentViewModel() { AddingTime = comment.dateTime, AuthorName = comment.User.UserName, ProblemId = problem.Id, Text = comment.Text };
                pvm.Comments.Add(com);
            }

            // pvm.Solved=problem.UsersWhoSolved.Contains()
           
            pvm.Likes = problem.Likes.Count;
            pvm.Dislikes = problem.Dislikes.Count;
            return pvm;
        }

        public void AddProblemFromView(ProblemViewModel problemView)
        {


            Problem problem = new Problem();
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            problem.AuthorId = UserManager.FindByName(User.Identity.Name).Id;
            problem.Tags = new Collection<Tag>();
            
            foreach(var tag in problemView.TagsString.Split(','))
            {
                problem.Tags.Add(tagRepository.GetByName(tag));
            }

            repository.Insert(problem);
            problem.ImagesInside = new Collection<Image>();
            if (problemView.Images != null)
                foreach (var image in problemView.Images)
                {
                    problem.ImagesInside.Add(new Image() { URL = image, ProblemId = problem.Id, UserId = problem.AuthorId });
                }
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            repository.Update(problem);
        }

        public void UpdateProblemFromView(ProblemViewModel problemView)
        {
            Problem problem = repository.GetByID(problemView.Id);
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            while (problem.CorrectAnswers.Count != 0)
                answerRepository.Delete(problem.CorrectAnswers.ElementAt(0));
            while (problem.ImagesInside.Count != 0)
                imageRepository.Delete(problem.ImagesInside.ElementAt(0));
            problem.ImagesInside = new Collection<Image>();
            foreach (var image in problemView.Images)
            {
                problem.ImagesInside.Add(new Image() { URL = image, ProblemId = problem.Id, UserId = problem.AuthorId });
            }
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            problem.Tags = new Collection<Tag>();
            repository.Update(problem);
            foreach (var tag in problemView.TagsString.Split(','))
            {
                problem.Tags.Add(tagRepository.GetByName(tag));
            }
            repository.Update(problem);
        }

        private ICollection<Answer> GetAnswersFromString(string answers, Problem problem)
        {
            string[] answersArray = answers.Split(';');

            var collection = new Collection<Answer>();
            foreach (var ans in answersArray)
            {
                ans.Trim();
                collection.Add(new Answer() { Text = ans, Problem = problem });
            }
            return collection;
        }

        private bool AreEqual(ICollection<Answer> collection1, ICollection<Answer> collection2)
        {
            if (collection1.Count != collection2.Count) return false;

            IEnumerable<string> c1 = from ans in collection1
                                     select ans.Text;
            IEnumerable<string> c2 = from ans in collection2
                                     select ans.Text;


            foreach (var ans in c1)
            {
                if (!c2.Contains(ans)) return false;
            }
            return true;
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