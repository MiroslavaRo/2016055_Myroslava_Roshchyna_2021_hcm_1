using System.Threading.Tasks;

namespace Hcm.Core.Database
{
    public interface IDatabaseContext
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
