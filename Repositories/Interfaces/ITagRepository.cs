using MyWebApp.Models;

namespace MyWebApp.Repositories.Interfaces
{
    public interface ITagRepository: IRepository<Tag>
    {
        Tag GetByName(string name);
    }
}
