using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime AddingTime { get; set; }
        public string AuthorName { get; set; }
        public int ProblemId { get; set; }
    }
}