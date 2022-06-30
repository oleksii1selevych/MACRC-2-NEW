using System.ComponentModel.DataAnnotations;

namespace Marc2.Contracts.Organization
{
    public class CreateOrganizationDto
    {
        [Required]
        [StringLength(200, ErrorMessage = "Organization name can not be longer than 200 characters")]
        public string OrganizationName { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = "Organization address can not be longer than 100 characters")]
        public string Address { get; set; } = null!;
        [Required]
        [StringLength(40, ErrorMessage = "Organization country name can not be longer than 40 characters")]
        public string Country { get; set; } = null!;
        [Required]
        [StringLength(40, ErrorMessage = "Organization city name can not be longer than 40 characters")]
        public string City { get; set; } = null!;
        [StringLength(2500, ErrorMessage = "Organization description can not be longer than 2500 characters")]
        public string? Description { get; set; }
    }
}
