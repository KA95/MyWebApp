using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
         public ImageRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}