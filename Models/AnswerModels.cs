namespace MyWebApp.Models
{
    public class Answer
    {

        public int Id { get; set; }
        public string Text { get; set; }

        public int ProblemId { get; set; }
        public virtual Problem Problem { get; set; }
    }

    
}