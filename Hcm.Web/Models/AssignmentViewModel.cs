using System;
using System.ComponentModel.DataAnnotations;

namespace Hcm.Web.Models
{
    public class AssignmentViewModel
    {
        [Display(Name = "Id")]
        public string AssignmentId { get; set; }
        [Display(Name = "Position")]
        public string JobTitle { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? Start { get; set; }
        [Display(Name = "End Date")]
        public DateTime? End { get; set; }
        [Display(Name = "Department Id")]
        public string DepartmentId { get; set; }
        [Display(Name = "Department")]
        public string Name { get; set; }
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Display(Name = "Currency")]
        public string Currency { get; set; }
        public (string Id, string Name)[] Departments { get; set; }
    }
}
