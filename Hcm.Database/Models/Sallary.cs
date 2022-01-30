using Hcm.Core.Database;

namespace Hcm.Database.Models
{
    public class Sallary : DatabaseModel
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
