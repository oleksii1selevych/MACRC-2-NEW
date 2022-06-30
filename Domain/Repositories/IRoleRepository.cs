using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> MapRoleListAsync(IEnumerable<int> roleIdentifiers);
        Task<IEnumerable<Role>> GetPossibleRolesAsync(int maxRolePriority);
    }
}
