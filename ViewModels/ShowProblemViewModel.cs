using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.ViewModels
{
    public class ShowProblemViewModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        
        public string Author { get; set; }



        public string Answers { get; set; }
       
        public string TagsString { get; set; }

        public ICollection<string> Tags { get; set; }

        public int Id { get; set; }
        public bool IsSolved { get; set; }
        public bool IsBlocked { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public string Category { get; set; }

        public List<CommentViewModel> Comments { get; set; }
    }
}