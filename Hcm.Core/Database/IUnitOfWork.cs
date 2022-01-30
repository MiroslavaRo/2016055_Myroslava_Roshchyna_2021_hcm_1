using System;
using System.Threading.Tasks;

namespace Hcm.Core.Database
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
