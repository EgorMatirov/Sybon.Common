using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sybon.Common
{
    public class BaseEntityRepository<TEntity, TContext> : IBaseEntityRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext Context;

        public BaseEntityRepository(TContext context)
        {
            Context = context;
        }

        public Task<TEntity> FindAsync(long key)
        {
            return Context.Set<TEntity>().FindAsync(key);
        }

        public async Task<long> AddAsync(TEntity entity)
        {
            var added = await Context.Set<TEntity>().AddAsync(entity);
            return added.Entity.Id;
        }
        
        public async Task RemoveAsync(long key)
        {
            var entity = await FindAsync(key);
            Context.Remove(entity);
        }
    }
}