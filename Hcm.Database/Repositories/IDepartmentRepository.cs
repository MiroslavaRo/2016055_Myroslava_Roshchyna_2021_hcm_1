using Hcm.Core.Database;
using Hcm.Database.Models;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> IsUniqueName(string name);

        Task<bool> IsUniqueName(string departmentId, string name);
    }
}
