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

        public ProblemController(IProblemRepository repository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository, IImageRepository imageRepository,IVideoRepository videoRepository, ITagRepository tagRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.answerRepository = answerRepository;
            this.userAttemptedRepository = userAttemptedRepository;
            this.userSolvedRepository = userSolvedRepository;
            this.tagRepository = tagRepository;
            this.imageRepository = imageRepository;
            this.videoRepository = videoRepository;
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
                if(AreEqual(GetAnswersFromString(problemView.Answers,problem),problem.CorrectAnswers))
                {
                    userSolvedRepository.Insert(new UserSolvedProblem(){ UserId = UserManager.FindByName(User.Identity.Name).Id , ProblemId= problem.Id});
                    pvm.Solved = true;
                }
                else
                {
                    userAttemptedRepository.Insert(new UserAttemptedProblem() { UserId = UserManager.FindByName(User.Identity.Name).Id, ProblemId = problem.Id });
                }


            Markdown md = new Markdown();

            problem.Text = md.Transform(problem.Text);

       
            
           
            pvm.Answers = "";
            return View(pvm);
        }

       

        [Authorize]
        public ActionResult Create()
        {

            ViewBag.Button = "Create";
            Problem problem = new Problem();
            return View(new ProblemViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(ProblemViewModel problemView)
        {
            if (ModelState.IsValidField("Answers"))
                AddProblemFromView(problemView);
            else
                return View(new ProblemViewModel());

            problemView.Author = User.Identity.Name;
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {

            ViewBag.Button = "Edit";
            Problem problem = repository.GetByID(id);

            return View(GetProblemViewModel(problem));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(ProblemViewModel problemView)
        {
            if (ModelState.IsValidField("Answers"))
                UpdateProblemFromView(problemView);
            else
                return View(new ProblemViewModel());

            problemView.Author = User.Identity.Name;
            return RedirectToAction("Index");
        }

        private ProblemViewModel GetProblemViewModel(Problem problem)
        {
            var pvm = new ProblemViewModel();
            pvm.Author = problem.Author.UserName;
            pvm.Name = problem.Name;
            pvm.Text = problem.Text;
            pvm.Id = problem.Id;
            pvm.Images = (from image in problem.CorrectAnswers
                               select image.Text);
            pvm.SelectedCategoryId = problem.CategoryId;
            StringBuilder sb = new StringBuilder();
            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }
            // pvm.Solved=problem.UsersWhoSolved.Contains()
            sb.Remove(sb.Length - 1, 1);
            pvm.Answers = sb.ToString();
            return pvm;
        }

        public void AddProblemFromView(ProblemViewModel problemView)
        {


            Problem problem = new Problem();
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            problem.AuthorId = UserManager.FindByName(User.Identity.Name).Id;
            repository.Insert(problem);
            problem.ImagesInside = new Collection<Image>();
            foreach(var image in problemView.Images)
            {
                problem.ImagesInside.Add(new Image(){URL = image , ProblemId = problem.Id, UserId = problem.AuthorId});
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
          
            ICollection<string> c1= new Collection<string>();
            ICollection<string> c2= new Collection<string>();
            foreach(var item in collection1)
                c1.Add(item.Text);
            foreach(var item in collection2)
                c2.Add(item.Text);
            if(c1.Except(c2).ToList().Count==0 && c2.Except(c1).ToList().Count==0)
                return true;
            else
                return false;
        }
    }
}