using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hcm.Web.Models
{
    public class EmployeeViewModel
    {
        [Display(Name = "Id")]
        public string EmployeeId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Family Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Address Line")]
        public string AddressLine { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }

        public AssignmentViewModel[] Assignments { get; set; }
    }
}
