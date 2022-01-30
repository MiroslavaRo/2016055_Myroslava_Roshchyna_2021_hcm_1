using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DatabaseContext context)
            : base(context)
        {

        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var exists = await Context.Set<Employee>()
                .Where(e => e.Email == email.Trim().ToLower())
                .Select(e => e.Id)
                .AnyAsync();

            return !exists;
        }

        public async Task<bool> IsEmailUniqueAsync(string employeeId, string email)
        {
            var exists = await Context.Set<Employee>()
               .Where(e => e.Email == email.Trim().ToLower())
               .Where(e => e.Id != employeeId.Trim().ToLower())
               .Select(e => e.Id)
               .AnyAsync();

            return !exists;
        }

        public async Task<bool> IsPhoneUniqueAsync(string phone)
        {
            var exists = await Context.Set<Employee>()
               .Where(e => e.Phone == phone.Trim().ToLower())
               .Select(e => e.Id)
               .AnyAsync();

            return !exists;
        }

        public async Task<bool> IsPhoneUniqueAsync(string employeeId, string phone)
        {
            var exists = await Context.Set<Employee>()
                  .Where(e => e.Phone == phone.Trim().ToLower())
                  .Where(e => e.Id != employeeId.Trim().ToLower())
                  .Select(e => e.Id)
                  .AnyAsync();

            return !exists;
        }
    }
}
