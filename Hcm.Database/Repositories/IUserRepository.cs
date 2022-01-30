using Hcm.Core.Database;
using Hcm.Database.Models;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAndRoleAsync(string username, Roles role);

        Task<bool> IsUniqueAsync(string email, string phone, Roles role);

        Task<bool> IsUniqueAsync(string userId, string email, string phone, Roles role);
    }
}
