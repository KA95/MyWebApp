using System.Collections.Generic;
using System.Text;
using MyWebApp.Models;
using MyWebApp.ViewModels;

namespace MyWebApp.Helpers
{
    public class GlobalHelper
    {
        public static ProblemViewModel GetProblemViewModel(Problem problem)
        {
            var pvm = new ProblemViewModel();
            pvm.Author = problem.Author.UserName;
            pvm.Name = problem.Name;
            pvm.Text = problem.Text;
            pvm.Id = problem.Id;
            pvm.SelectedCategoryId = problem.CategoryId;
            pvm.Category = problem.Category.Name;

            var sb = new StringBuilder();
            foreach (var tag in problem.Tags)
            {
                sb.Append(tag.Name);
                sb.Append(',');
            }

            pvm.TagsString = sb.ToString();

            sb.Clear();

            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }
            sb.Remove(sb.Length - 1, 1);
            pvm.Answers = sb.ToString();

            pvm.Comments = new List<CommentViewModel>();
            foreach (var comment in problem.Comments)
            {
                CommentViewModel com = new CommentViewModel() { AddingTime = comment.dateTime, AuthorName = comment.User.UserName, ProblemId = problem.Id, Text = comment.Text };
                pvm.Comments.Add(com);
            }

            // pvm.Solved=problem.UsersWhoSolved.Contains()

            pvm.Likes = problem.Likes.Count;
            pvm.Dislikes = problem.Dislikes.Count;
            return pvm;
        }

    }
}