using System.Text.Json.Serialization;

namespace Marc2.Contracts.Organization
{
    public class OrganizationDto
    {
        public int OrganizationId { get; set; }
        [JsonPropertyName("organizationName")]
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Description { get; set; }
        public int UsersQuantity { get; set; }
        public int SmartDevicesQuantity { get; set; }
    }
}
