using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        { }

        public void CreateUser(User user)
            => Create(user);

        public void DeleteUser(User user)
            => Delete(user);

        public async Task<User?> GetByEmailAsync(string email)
           => await FindByCondition(u => u.Email == email).Include(u => u.Roles).Include(u => u.Organization).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetPrioritizedUsersByOrganizationAsync(int organizationId, User perpetrator)
        {
            var perpetratorMaxPriority = perpetrator.Roles.Max(r => r.RolePriority);
            var users = await FindByCondition(u => (u.Organization.Id == organizationId &&
            u.Roles.Max(r => r.RolePriority) < perpetratorMaxPriority) 
            || (u.Organization == null && perpetrator.Organization == null) &&
            (u.Id != perpetrator.Id))
                .Include(u => u.Organization).AsNoTracking().ToListAsync();

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
            => await FindByCondition(u => u.Id == userId).Include(u => u.Roles).Include(u => u.Organization).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetUsersByOrganizationAsync(int organizationId, int perpetratorId)
        {
            if (organizationId == 0)
                return await FindByCondition(u => u.Organization == null && u.Id != perpetratorId).Include(u => u.Roles).ToListAsync();
            else
                return await FindByCondition(u => u.Organization.Id == organizationId && u.Id != perpetratorId).Include(u => u.Organization).
                   Include(u => u.Roles).ToListAsync();
        }

        public void UpdateUser(User user)
            => Update(user);
    }
}
