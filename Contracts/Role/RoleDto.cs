using System.Text.Json.Serialization;

namespace Marc2.Contracts.Role
{
    public class RoleDto
    {
        [JsonPropertyName("roleId")]
        public int Id { get; set; }
        [JsonPropertyName("roleName")]
        public string Name { get; set; } = null!;
        public int RolePriority { get; set; }
    }
}
