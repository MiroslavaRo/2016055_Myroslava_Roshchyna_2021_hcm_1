using Hcm.Core.Database;
using System.Collections.Generic;

namespace Hcm.Database.Models
{
    public class Employee : DatabaseModel
    {
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set;}
        public virtual IEnumerable<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
