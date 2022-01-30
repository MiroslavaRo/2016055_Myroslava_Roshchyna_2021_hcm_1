using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context)
            : base(context)
        {
        }

        public async Task<User> GetByUsernameAndRoleAsync(string username, Roles role)
        {
            return await Context.Set<User>()
                .Where(e => e.Username == username.ToLower().Trim())
                .Where(e => e.Role == role)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> IsUniqueAsync(string email, string phone, Roles role)
        {
            var exists = await Context.Set<User>()
               .Where(e => (e.Email == email.Trim().ToLower() 
                    || e.Phone == phone.Trim().ToLower()) 
                        && e.Role == role)
               .Select(e => e.Id)
               .AnyAsync();

            return !exists;
        }

        public async Task<bool> IsUniqueAsync(string userId, string email, string phone, Roles role)
        {
            var exists = await Context.Set<User>()
             .Where(e => (e.Email == email.Trim().ToLower()
                  || e.Phone == phone.Trim().ToLower())
                      && e.Role == role
                      && e.Id != userId.Trim().ToLower())
             .Select(e => e.Id)
             .AnyAsync();

            return !exists;
        }
    }
}
