using System.ComponentModel.DataAnnotations;

namespace Hcm.Api.Dto
{
    public class DepartmentCreateDto
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class DepartmentUpdateDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class DepartmentDto
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
    }
}
