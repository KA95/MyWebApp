namespace MyWebApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        //public int Id { get; set; }
        [Key]
        public string Name { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }
    }
  
}