using Hcm.Core.Database;
using System.Collections.Generic;

namespace Hcm.Database.Models
{
    public class Department : DatabaseModel
    {
        public string Country { get; set; }
        public string City { get; set; }     
        public string Name { get; set; }

        public virtual IEnumerable<Assignment> Assignments { get; set; }
    }
}
