using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Organization?> GetOrganizationByIdAsync(int organizationId)
            => await FindByCondition(o => o.Id == organizationId).Include(o => o.Accidents).Include(o => o.Users)
            .Include(o => o.SmartBracelets).FirstOrDefaultAsync();

        public void CreateOrganization(Organization organization)
            => Create(organization);
        public void UpdateOrganization(Organization organization)
            => Update(organization);
        public void DeleteOrganization(Organization organization)
            => Delete(organization);

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
            => await FindAll().Include(o => o.Users).Include(s => s.SmartBracelets).AsNoTracking().ToListAsync();
    }
}
