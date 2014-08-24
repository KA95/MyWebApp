using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWebApp.Models;

namespace MyWebApp.ViewModels
{
    public class HomepageViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }
        public IList<HomepageProblem> RecentProblems { get; set; }
        public IList<HomepageProblem> PopularProblems { get; set; }
        public IList<HomepageUser> TopUsers { get; set; }

    }
}