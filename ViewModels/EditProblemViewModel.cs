using System.Web;
using MyWebApp.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyWebApp.Models;
using System.Web.Mvc;
using MyWebApp.ValidationAttributes;

namespace MyWebApp.ViewModels
{
    public class EditProblemViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [CorrectSetOfAnswers]
        [Display(Name = "Answers")]
        public string Answers { get; set; }

        public int Id { get; set; }

        public bool IsBlocked { get; set; }

        [Display(Name = "Tags")]
        public string TagsString { get; set; }

        public ICollection<string> Tags { get; set; }

        private readonly List<Category> _categories = new CategoryRepository(new ApplicationDbContext()).Get().ToList();

        public HttpPostedFileBase MyFile { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories
        {
            get { return new SelectList(_categories, "Id", "Name"); }
        }
    }
}