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

namespace MyWebApp.Models
{
    public class ProblemViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Author { get; set; }
        //[Required]
        //IEnumerable<string> Answers { get; set; }

        IEnumerable<string> Tags { get; set; }
        IEnumerable<Category> Categories { get; set; }

        GenericRepository<Category> categoryRepository = new GenericRepository<Category>();
        
        ProblemViewModel(Problem problem)
        {
            Name = problem.Name;
            Text = problem.Text;
            Category = problem.Category.Name;
            Author = problem.Author.UserName;
            //Answers = (from item in problem.CorrectAnswers
            //           select item.Text);
            Tags = (from item in problem.Tags
                       select item.Name);
            Categories = categoryRepository.Get();

        }
        public Problem GetProblem()
        {
            Problem problem = new Problem();
            problem.Category = categoryRepository.Get(e=> e.Name == Category).First();
            problem.CategoryId = problem.Category.Id;
            problem.Name = Name;
            problem.Text = Text;
            return problem;

        }
    }
}