using Hcm.Database.Models;

namespace Hcm.Database.Repositories
{
    public class SallaryRepository : Repository<Sallary>, ISallaryRepository
    {
        public SallaryRepository(DatabaseContext context)
            : base(context)
        {

        }
    }
}
