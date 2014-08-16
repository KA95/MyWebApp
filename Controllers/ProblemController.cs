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


        public ProblemController(IProblemRepository repository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.answerRepository = answerRepository;
            this.userAttemptedRepository = userAttemptedRepository;
            this.userSolvedRepository = userSolvedRepository;
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

            var pvm = GetProblemViewModel(problem);
            pvm.Answers = "";
            return View(pvm);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Show(ProblemViewModel problemView)
        {
            int a = 0;
            if (ModelState.IsValidField("Answers"))
                a++;

            Problem problem = repository.GetByID(problemView.Id);

            Markdown md = new Markdown();

            problem.Text = md.Transform(problem.Text);

            var pvm = GetProblemViewModel(problem);
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

            string answers = problemView.Answers;
            string[] answersArray = answers.Split(';');

            var collection = new Collection<Answer>();
            foreach (var ans in answersArray)
            {
                ans.Trim();
                collection.Add(new Answer() { Text = ans, Problem = problem });
            }
            problem.CorrectAnswers = collection;
            repository.Update(problem);
        }

        public void UpdateProblemFromView(ProblemViewModel problemView)
        {


            Problem problem = repository.GetByID(problemView.Id);
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;

            while(problem.CorrectAnswers.Count!=0)
                answerRepository.Delete(problem.CorrectAnswers.ElementAt(0));

            problem.CorrectAnswers.Clear();
         
            string answers = problemView.Answers;
            string[] answersArray = answers.Split(';');

            foreach (var ans in answersArray)
            {
                ans.Trim();
                problem.CorrectAnswers.Add(new Answer() { Text = ans, Problem = problem , ProblemId=problem.Id });
            }
            repository.Update(problem);
        }

    }
}