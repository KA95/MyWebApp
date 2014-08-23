using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class ProblemRepository: GenericRepository<Problem>,IProblemRepository
    {
        public ProblemRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}