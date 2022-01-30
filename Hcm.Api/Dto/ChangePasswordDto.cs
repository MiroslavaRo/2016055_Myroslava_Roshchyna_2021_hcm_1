using System.ComponentModel.DataAnnotations;

namespace Hcm.Api.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
