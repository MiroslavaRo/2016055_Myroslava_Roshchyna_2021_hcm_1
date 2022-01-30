using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Api.Dto
{
    public class AssignmentDto
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }

        public string JobTitle { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        
        public SallaryDto Sallary { get; set; }

        public IEnumerable<SallaryDto> SallariesHistory { get; set; }
    }

    public class AssignmentCreateDto
    {
        public string JobTitle { get; set; }
        [Required]
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [Required]
        public SallaryDto Sallary { get; set; }
    }

    public class AssignmentUpdateDto
    {
        [Required]
        public string Id { get; set; }
        public string JobTitle { get; set; }
        [Required]
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [Required]
        public SallaryDto Sallary { get; set; }
    }

    public class SallaryDto
    {
        [Required]
        public string Currency { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
