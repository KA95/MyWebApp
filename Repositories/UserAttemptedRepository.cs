using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

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