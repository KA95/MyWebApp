namespace MyWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Problem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<UserSolvedProblem> UsersWhoSolved { get; set; }
        public virtual ICollection<UserAttemptedProblem> UsersWhoAttempted { get; set; }
        public virtual ICollection<Image> ImagesInside { get; set; }
        public virtual ICollection<Video> VideosInside { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        

    }
}