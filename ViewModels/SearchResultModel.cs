using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWebApp.Models;

namespace MyWebApp.ViewModels
{
    public class SearchResultModel
    {
        public IList<ApplicationUser> Users { get; set; }
        public IList<Problem> Problems  { get; set; }
    }
}