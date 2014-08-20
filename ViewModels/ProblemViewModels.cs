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
    public class ProblemViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [CorrectSetOfAnswers]
        public string Answers { get; set; }

        public ICollection<string> Tags { get; set; }
        public ICollection<string> Images { get; set; }
        public ICollection<string> Videos { get; set; }

        public int Id { get; set; }
        public bool Solved { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public string Category{ get; set; }

        public List<CommentViewModel> Comments { get; set; }
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