using System;
using System.Threading.Tasks;

namespace Sybon.Common
{
    public interface IRepositoryUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}