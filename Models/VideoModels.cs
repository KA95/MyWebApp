namespace MyWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Video
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}