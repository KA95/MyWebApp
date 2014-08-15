using MyWebApp.Controllers;
using MyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web;
using MyWebApp.Models;
using System.Web.Mvc;
using MyWebApp.ValidationAttributes;

namespace MyWebApp.ViewModels
{
    public class CreateProblemViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [CorrectSetOfAnswers(ErrorMessage="Incorrect answer set.")]
        public string Answers { get; set; }
        //[Required]
        //public List<string> Tags { get; set; }

        private readonly List<Category> _categories = new CategoryRepository(new ApplicationDbContext()).Get().ToList();

        [Required]
        [Display(Name = "Category")]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories
        {
            get { return new SelectList(_categories, "Id", "Name"); }
        }
    }
}