﻿using MyWebApp.Controllers;
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

namespace MyWebApp.ViewModels
{
    public class CreateProblemViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        //[Required]
        //public string Category { get; set; }
        [Required]
        public string Author { get; set; }
        //[Required]
        //IEnumerable<string> Answers { get; set; }

        public IEnumerable<string> Tags { get; set; }

        private readonly List<Category> _categories = new CategoryRepository(new ApplicationDbContext()).Get().ToList();

        [Display(Name = "Category")]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories
        {
            get { return new SelectList(_categories, "Id", "Name"); }
        }
    }
}