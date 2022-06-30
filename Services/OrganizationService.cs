using Marc2.Contracts.Organization;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepositoryManager _repository;
        public OrganizationService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationDto> GetOrganizationByIdAsync(int organizationId)
        {
            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(organizationId);

            return MapFromOrganizationToDto(organization);
        }


        public async Task<IEnumerable<OrganizationDto>> GetAllOrganizationsAsync()
        {
            var organizations = await _repository.OrganizationRepository.GetAllOrganizationsAsync();
            return organizations.Select(o => MapFromOrganizationToDto(o)).ToList();
        }


        public async Task<OrganizationDto> CreateOrganizationAsync(CreateOrganizationDto createOrganizationDto)
        {
            var organization = MapFromCreateOrganizationDto(createOrganizationDto);

            _repository.OrganizationRepository.CreateOrganization(organization);
            await _repository.UnitOfWork.SaveChangesAsync();

            return MapFromOrganizationToDto(organization);
        }

        public async Task UpdateOrganizationAsync(int organizationId, UpdateOrganizationDto updateOrganizationDto)
        {
            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(organizationId);

            MapFromUpdateOrganizationDto(updateOrganizationDto, organization);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrganizationAsync(int organizationId)
        {
            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(organizationId);

            foreach (var user in organization.Users)
                _repository.UserRepository.DeleteUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();

            foreach (var accident in organization.Accidents)
                _repository.AccidentRepository.DeleteAccident(accident);

            await _repository.UnitOfWork.SaveChangesAsync();

            foreach (var smartDevice in organization.SmartBracelets)
                _repository.SmartBraceletRepository.DeleteSmartBracelet(smartDevice);
            await _repository.UnitOfWork.SaveChangesAsync();

            _repository.OrganizationRepository.DeleteOrganization(organization);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        private void MapFromUpdateOrganizationDto(UpdateOrganizationDto updateOrganizationDto, Organization organization)
        {
            organization.Address = updateOrganizationDto.Address;
            organization.Name = updateOrganizationDto.OrganizationName;
            organization.City = updateOrganizationDto.City;
            organization.Description = updateOrganizationDto.Description;
            organization.Country = updateOrganizationDto.Country;
        }

        private Organization MapFromCreateOrganizationDto(CreateOrganizationDto createOrganizationDto)
        {
            var organization = new Organization
            {
                Name = createOrganizationDto.OrganizationName,
                Address = createOrganizationDto.Address,
                City = createOrganizationDto.City,
                Country = createOrganizationDto.Country,
                Description = createOrganizationDto.Description
            };
            return organization;
        }

        private OrganizationDto MapFromOrganizationToDto(Organization organization)
        {
            var organizationDto = new OrganizationDto
            {
                OrganizationId = organization.Id,
                Name = organization.Name,
                Address = organization.Address,
                Country = organization.Country,
                City = organization.City,
                Description = organization.Description,
                UsersQuantity = organization.Users != null ? organization.Users.Count : 0,
                SmartDevicesQuantity = organization.SmartBracelets != null ? organization.SmartBracelets.Count : 0
            };
            return organizationDto;
        }
    }
}
