using Hcm.Core.Database;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isCommited = false;
        private readonly IDatabaseContext _context;

        public UnitOfWork(IDatabaseContext context)
        {
            _context = context;
        }

        public Task CommitAsync()
        {
            if (_isCommited)
            {
                return Task.CompletedTask;
            }

            _isCommited = true;
            return _context.CommitAsync();
        }

        public void Dispose()
        {
            RollbackAsync()
                .GetAwaiter()
                .GetResult();
        }

        public Task RollbackAsync()
        {
            if (_isCommited)
            {
                return Task.CompletedTask;
            }

            return _context.RollbackAsync();
        }
    }
}
