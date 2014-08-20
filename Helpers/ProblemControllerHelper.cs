using MyWebApp.Models;
using MyWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;

namespace MyWebApp.Helpers
{
    public class ProblemControllerHelper
    {

        private static ProblemViewModel GetProblemViewModel(Problem problem)
        {
            var pvm = new ProblemViewModel();
            pvm.Author = problem.Author.UserName;
            pvm.Name = problem.Name;
            pvm.Text = problem.Text;
            pvm.Id = problem.Id;
            pvm.Images = (from image in problem.CorrectAnswers
                          select image.Text);
            pvm.SelectedCategoryId = problem.CategoryId;
            StringBuilder sb = new StringBuilder();
            foreach (var ans in problem.CorrectAnswers)
            {
                sb.Append(ans.Text);
                sb.Append(';');
            }
            // pvm.Solved=problem.UsersWhoSolved.Contains()
            sb.Remove(sb.Length - 1, 1);
            pvm.Answers = sb.ToString();
            pvm.Likes = problem.Likes.Count;
            pvm.Dislikes = problem.Dislikes.Count;
            return pvm;
        }

        public static void AddProblemFromView(ProblemViewModel problemView)
        {


            Problem problem = new Problem();
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            problem.AuthorId = UserManager.FindByName(User.Identity.Name).Id;
            repository.Insert(problem);
            problem.ImagesInside = new Collection<Image>();
            if (problemView.Images != null)
                foreach (var image in problemView.Images)
                {
                    problem.ImagesInside.Add(new Image() { URL = image, ProblemId = problem.Id, UserId = problem.AuthorId });
                }
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            repository.Update(problem);
        }

        public static void UpdateProblemFromView(ProblemViewModel problemView)
        {
            Problem problem = repository.GetByID(problemView.Id);
            problem.CategoryId = problemView.SelectedCategoryId;
            problem.Name = problemView.Name;
            problem.Text = problemView.Text;
            while (problem.CorrectAnswers.Count != 0)
                answerRepository.Delete(problem.CorrectAnswers.ElementAt(0));
            while (problem.ImagesInside.Count != 0)
                imageRepository.Delete(problem.ImagesInside.ElementAt(0));
            problem.ImagesInside = new Collection<Image>();
            foreach (var image in problemView.Images)
            {
                problem.ImagesInside.Add(new Image() { URL = image, ProblemId = problem.Id, UserId = problem.AuthorId });
            }
            problem.CorrectAnswers = GetAnswersFromString(problemView.Answers, problem);
            repository.Update(problem);
        }

        public static ICollection<Answer> GetAnswersFromString(string answers, Problem problem)
        {
            string[] answersArray = answers.Split(';');

            var collection = new Collection<Answer>();
            foreach (var ans in answersArray)
            {
                ans.Trim();
                collection.Add(new Answer() { Text = ans, Problem = problem });
            }
            return collection;
        }

        public static bool AreEqual(ICollection<Answer> collection1, ICollection<Answer> collection2)
        {
            if (collection1.Count != collection2.Count) return false;

            IEnumerable<string> c1 = from ans in collection1
                                     select ans.Text;
            IEnumerable<string> c2 = from ans in collection2
                                     select ans.Text;


            foreach (var ans in c1)
            {
                if (!c2.Contains(ans)) return false;
            }
            return true;
        }
    }
}