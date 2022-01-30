using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Api.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class CreateAdministratorDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class UpdateAdministratorDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
