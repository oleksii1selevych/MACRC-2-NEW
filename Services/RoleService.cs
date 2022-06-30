using Marc2.Contracts.Role;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repository;
        public RoleService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllPossibleRolesAsync(string userEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);
            if (user == null)
                throw new UserNotFoundException(userEmail);

            int maxUserRolePriority = user.Roles.Max(r => r.RolePriority);
            var roles = await _repository.RoleRepository.GetPossibleRolesAsync(maxUserRolePriority);
            return roles.Select(r => new RoleDto { Id = r.Id, Name = r.RoleName, RolePriority = r.RolePriority });
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);
            if (user == null)
                throw new UserNotFoundException(userEmail);
            return user.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.RoleName, RolePriority = r.RolePriority }).AsEnumerable();
        }
    }
}
