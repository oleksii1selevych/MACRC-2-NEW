using Marc2.Contracts.Organization;

namespace Marc2.Services.Abstractions
{
    public interface IOrganizationService
    {
        Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto createOrganizationDto);
        Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync();
        Task<OrganizationDto> GetOrganizationByIdAsync(int organizationId);
        Task UpdateOrganizationAsync(int organizationId, UpdateOrganizationDto updateOrganizationDto);
        Task DeleteOrganizationAsync(int organizationId);
    }
}
