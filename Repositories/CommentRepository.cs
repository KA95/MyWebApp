using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class CommentRepository: GenericRepository<Comment>,ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}