using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class AnswerRepository :GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}