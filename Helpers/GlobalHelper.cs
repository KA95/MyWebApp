using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWebApp.Models;
using MyWebApp.ViewModels;

namespace MyWebApp.Helpers
{
    public class GlobalHelper
    {
        public static EditProblemViewModel GetEditProblemViewModel(Problem problem)
        {
            var epvm = new EditProblemViewModel();
            epvm.Author = problem.Author.UserName;
            epvm.Name = problem.Name;
            epvm.Text = problem.Text;
            epvm.SelectedCategoryId = problem.CategoryId;
            epvm.IsBlocked = problem.IsBlocked;

            var sb = new StringBuilder();
            foreach (var tag in problem.Tags)
            {
                sb.Append(tag.Name);
                sb.Append(',');
            }

            epvm.TagsString = sb.ToString();

            sb.Clear();

            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }

            sb.Remove(sb.Length - 1, 1);
            epvm.Answers = sb.ToString();

            return epvm;
        }

        public static ShowProblemViewModel GetShowProblemViewModel(Problem problem)
        {
            var spvm = new ShowProblemViewModel();
            spvm.Author = problem.Author.UserName;
            spvm.Name = problem.Name;
            spvm.Text = problem.Text;
            spvm.Id = problem.Id;
            spvm.Category = problem.Category.Name;
            spvm.IsBlocked = problem.IsBlocked;
            var sb = new StringBuilder();
            foreach (var tag in problem.Tags)
            {
                sb.Append(tag.Name);
                sb.Append(',');
            }

            spvm.TagsString = sb.ToString();

            sb.Clear();

            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }

            sb.Remove(sb.Length - 1, 1);
            spvm.Answers = sb.ToString();

            spvm.Comments = new List<CommentViewModel>();
            foreach (var comment in problem.Comments)
            {
                CommentViewModel com = new CommentViewModel() { AddingTime = comment.dateTime, AuthorName = comment.User.UserName, ProblemId = problem.Id, Text = comment.Text };
                spvm.Comments.Add(com);
            }

            spvm.Likes = problem.Likes.Count;
            spvm.Dislikes = problem.Dislikes.Count;
            return spvm;
        }
        public static bool AreEqual(ICollection<Answer> collection1, ICollection<Answer> collection2)
        {
            if (collection1.Count != collection2.Count) return false;

            IEnumerable<string> c1 = from ans in collection1
                                     select ans.Text;
            IEnumerable<string> c2 = from ans in collection2
                                     select ans.Text;
            return c1.All(c2.Contains);
        }
    }
}