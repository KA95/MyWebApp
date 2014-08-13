using MyWebApp.Models;
using MyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MyWebApp.Controllers
{
    class MyViewModel
    {
        Problem problem { get; set; }
        IEnumerable<Category> categories { get; set; }

        public MyViewModel(Problem problem)
        {
            // TODO: Complete member initialization
            this.problem = problem;
            var repository = new GenericRepository<Category>();
            categories = repository.Get();
        }
    }

    public class ProblemController : Controller
    {
        private GenericRepository<Problem> repository = new GenericRepository<Problem>();
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
            Problem problem= new Problem();
            return View(new MyViewModel(problem));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(ProblemViewModel problem)
        {
        
            if (problem.Name == null || problem.Name == "")
                RedirectToAction("Index");
            problem.Author = User.Identity.Name;
            
            repository.Insert(problem.GetProblem());

            return RedirectToAction("Index");
        }
    }
}