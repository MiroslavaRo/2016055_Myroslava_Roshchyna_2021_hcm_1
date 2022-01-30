using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Api.Dto
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }

    public class EmployeeCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}
