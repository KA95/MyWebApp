namespace MyWebApp.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProblemId { get; set; }
        public virtual Problem Problem { get; set; }
    }
   
}