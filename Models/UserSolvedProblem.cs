using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class UserSolvedProblem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Problem Problem { get; set; }
    }
 
}
