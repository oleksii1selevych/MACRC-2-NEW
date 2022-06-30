using Marc2.Contracts.Role;

namespace Marc2.Services.Abstractions
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userEmail);
        Task<IEnumerable<RoleDto>> GetAllPossibleRolesAsync(string userEmail);
    }
}
