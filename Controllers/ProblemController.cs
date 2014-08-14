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

namespace MyWebApp.Controllers
{
   
    public class ProblemController : Controller
    {

        private readonly IProblemRepository repository;
        private readonly ICategoryRepository categoryRepository;


        public ProblemController(IProblemRepository repository, CategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
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

            return View(problem);
        }
        /*
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.Button = "Save changes";
            ViewBag.Images = GetImageCollection();

            ViewBag.SlideshowImages = (from image in SlideshowImageContext.SlideshowImages
                                       where image.Slideshow.Id == id
                                       orderby image.ImageSerialNumber
                                       select image.Image);

            return View(dbcontext.Slideshows.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(Slideshow slideshow, string array)
        {

            dbcontext.Entry(slideshow).State = System.Data.Entity.EntityState.Deleted;

            int[] imageOrder = JsonConvert.DeserializeObject<int[]>(array);

            dbcontext.SaveChanges();

            for (int i = 0; i < imageOrder.Length; i++)
            {
                SlideshowImage it = new SlideshowImage();
                it.Image = dbcontext.Images.Find(imageOrder[i]);
                it.ImageSerialNumber = i;
                it.Slideshow = slideshow;
                SlideshowImageContext.Entry(it).State = System.Data.Entity.EntityState.Added;
            }

            slideshow.User = User.Identity.Name;
            dbcontext.Entry(slideshow).State = System.Data.Entity.EntityState.Added;
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }*/
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.Button = "Create";
            Problem problem = new Problem();
            CreateProblemViewModel pvm = new CreateProblemViewModel();
            return View(pvm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateProblemViewModel problemView)
        {

            if (problemView.Name == null || problemView.Name == "" || problemView.Text == null || problemView.Text == "")
                RedirectToAction("Index");

            problemView.Author = User.Identity.Name;

            repository.Insert(GetProblem(problemView));
            

            return RedirectToAction("Index");
        }





        public Problem GetProblem(CreateProblemViewModel problemView)
        {
            Problem problem = new Problem();
            //problem.Category = categoryRepository.GetByID(problemView.SelectedCategoryId);
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            //problem.Author = UserManager.FindByName(User.Identity.Name);
            problem.AuthorId = UserManager.FindByName(User.Identity.Name).Id;
            return problem;

        }

       
    }
}