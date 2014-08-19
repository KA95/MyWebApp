using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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