using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetPossibleRolesAsync(int maxRolePriority)
            => await FindByCondition(r => r.RolePriority <= maxRolePriority).ToListAsync();

        public async Task<List<Role>> MapRoleListAsync(IEnumerable<int> roleIdentifiers)
            => await FindByCondition(r => roleIdentifiers.Contains(r.Id)).ToListAsync();
    }
}
