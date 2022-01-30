using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DatabaseContext context)
            : base(context)
        {
            
        }

        public async Task<bool> IsUniqueName(string name)
        {
            var result = await Context.Set<Department>()
                .Where(e => e.Name.ToLower() == name.ToLower().Trim())
                .Select(e => e.Id)
                .AnyAsync();

            return !result;
        }

        public async Task<bool> IsUniqueName(string departmentId, string name)
        {
            var result = await Context.Set<Department>()
                .Where(e => e.Name.ToLower() == name.ToLower().Trim())
                .Where(e => e.Id != departmentId.ToLower().Trim())
                .Select(e => e.Id)
                .AnyAsync();

            return !result;
        }
    }
}
