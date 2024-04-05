namespace FlyrSupermarket.Infrastructure.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(object id);
    }
}