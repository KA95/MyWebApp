namespace MyWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Image
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}