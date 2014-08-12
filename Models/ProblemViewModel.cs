using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public int CategoryId { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        IEnumerable<string> Answers { get; set; }
        IEnumerable<string> Tags { get; set; }

        //private GenericRepository<Problem> problemRepository = new GenericRepository<Problem>();
        //private GenericRepository<> problemRepository = new GenericRepository<Problem>();
        ProblemViewModel(Problem problem)
        {
            Name = problem.Name;
            Text = problem.Text;
            CategoryId = problem.CategoryId;
            AuthorId = problem.AuthorId;
            Answers = (from item in problem.CorrectAnswers
                       select item.Text);
            Tags = (from item in problem.Tags
                       select item.Name);

 

        }
        Problem GetProblem()
        {
            Problem problem = new Problem();
            problem
            return null;

        }
    }
}