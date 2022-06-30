using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetOrganizationByIdAsync(int organizationId);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        void CreateOrganization(Organization organization);
        void UpdateOrganization(Organization organization);
        void DeleteOrganization(Organization organization);
    }
}
