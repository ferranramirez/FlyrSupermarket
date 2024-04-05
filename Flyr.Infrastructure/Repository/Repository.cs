using Microsoft.EntityFrameworkCore;

namespace FlyrSupermarket.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;

        public virtual TEntity Get(object id)
        {
            return DbSet.Find(id);
        }
    }
}
