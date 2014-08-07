namespace MyWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Problem Problem { get; set; }
    }
}