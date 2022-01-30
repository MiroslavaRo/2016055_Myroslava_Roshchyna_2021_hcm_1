using System;
using System.ComponentModel.DataAnnotations;

namespace Hcm.Api.Dto
{
    public class TokenRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Range(1, 2)]
        public int? Role { get; set; }
    }

    public class TokenResultDto
    {
        public string Token { get; set; }
        public string Schema { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
