using FlySupermarket.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlyrSupermarket.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly FlyrContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(FlyrContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = _context.Set<TEntity>();
        }

        public Repository() { }

        public virtual TEntity Get(object id)
        {
            return DbSet.Find(id);
        }
    }
}
