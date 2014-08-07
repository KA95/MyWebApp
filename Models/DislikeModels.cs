namespace MyWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Dislike
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Problem Problem { get; set; }

    }
}