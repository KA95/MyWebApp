using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyWebApp.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        TEntity GetByID(object id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
