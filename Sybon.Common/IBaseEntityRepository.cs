using System.Threading.Tasks;

namespace Sybon.Common
{
    public interface IBaseEntityRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> FindAsync(long key);
        Task<long> AddAsync(TEntity entity);
        Task RemoveAsync(long key);
    }
}