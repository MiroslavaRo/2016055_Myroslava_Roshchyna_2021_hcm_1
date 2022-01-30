using Hcm.Database.Models;

namespace Hcm.Database.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(DatabaseContext context)
            : base(context)
        {

        }
    }
}
