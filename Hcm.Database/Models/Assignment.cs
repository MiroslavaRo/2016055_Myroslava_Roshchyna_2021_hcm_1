using Hcm.Core.Database;
using System;
using System.Collections.Generic;

namespace Hcm.Database.Models
{
    public class Assignment : DatabaseModel
    {
        public string JobTitle { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Department Department { get; set; }
        public virtual IEnumerable<Sallary> Sallaries { get; set; }
    }
}
