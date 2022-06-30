using Marc2.Contracts.Role;

namespace Marc2.Contracts.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int OrganizationId { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; } = null!;
    }
}
