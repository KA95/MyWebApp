using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class UserAttemptedRepository : GenericRepository<UserAttemptedProblem>, IUserAttemptedRepository
    {
        public UserAttemptedRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}