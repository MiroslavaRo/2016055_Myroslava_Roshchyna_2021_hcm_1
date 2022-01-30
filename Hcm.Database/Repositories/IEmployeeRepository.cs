using Hcm.Core.Database;
using Hcm.Database.Models;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsEmailUniqueAsync(string employeeId, string email);
        Task<bool> IsPhoneUniqueAsync(string phone);
        Task<bool> IsPhoneUniqueAsync(string employeeId, string phone);
    }
}
