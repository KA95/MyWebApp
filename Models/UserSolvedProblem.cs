using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class UserSolvedProblem
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Problem Problem { get; set; }
    }
}
