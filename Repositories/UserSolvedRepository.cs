using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class UserSolvedRepository :GenericRepository<UserSolvedProblem>, IUserSolvedRepository
    {
        public UserSolvedRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}