using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class DislikeRepository : GenericRepository<Dislike>, IDislikeRepository
    {
        public DislikeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}