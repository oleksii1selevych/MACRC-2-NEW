using Marc2.Contracts.Auth;
using Marc2.Contracts.Role;
using Marc2.Contracts.User;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Helpers;
using Marc2.Services.Abstractions;
using System.Security.Claims;

namespace Marc2.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        public UserService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(UserAuthDto userAuthDto)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userAuthDto.Email);
            if (user == null)
                throw new InvalidUserCredentialsException();

            if(!PasswordHelper.CheckPassword(user.HashedPassword, userAuthDto.Password))
                throw new InvalidUserCredentialsException();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name,  String.Format("{0} {1}", user.FirstName, user.LastName))
            };

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

            return claims;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersByOrganizationAsync(string perpetratorEmail, int organizationId = 0)
        {
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            if (!(IsUserInAdminRole(perpetrator) || perpetrator.Organization.Id == organizationId))
                throw new AmbigousOrganizationException();

            var users = await _repository.UserRepository.GetPrioritizedUsersByOrganizationAsync(organizationId, perpetrator);
            var dtos = users.ToList().Select(u => MapFromUserToDto(u)).AsEnumerable();

            return dtos;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string perpetratorEmail, int organizationId = 0)
        {
            var user = await MapFromCreateUserDtoAsync(createUserDto);
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            if (!IsPerperatorPowered(perpetrator, user))
                throw new AbuseOfPowerException();

            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
            if (organization == null && perpetrator.Organization != null)
                throw new OrganizationNotFoundException(organizationId);

            user.Organization = organization;
            if (!IsOrganizationValid(perpetrator, user))
                throw new AmbigousOrganizationException();

            bool emailExists = await _repository.UserRepository.GetByEmailAsync(user.Email) != null;
            if (emailExists)
                throw new UserEmailExistsExeption(user.Email);

            string password = String.Format("{0}.12345", user.Email);
            string passwordHash = PasswordHelper.Hash(password);
            user.HashedPassword = passwordHash;

            _repository.UserRepository.CreateUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();

            return MapFromUserToDto(user);

        }

        public async Task UpdateUserAsync(UpdateUserDto updateUserDto, string perpetratorEmail, int userId)
        {
            var user = await _repository.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(userId);

            await MapFromUpdateUserDto(updateUserDto, user);
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            if (!IsPerperatorPowered(perpetrator, user))
                throw new AbuseOfPowerException();

            if (!IsOrganizationValid(perpetrator, user))
                throw new AmbigousOrganizationException();

            _repository.UserRepository.UpdateUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();
        }


        public async Task DeleteUserAsync(int userId, string perpetratorEmail)
        {
            var user = await _repository.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(userId);

            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            if (!IsPerperatorPowered(perpetrator, user))
                throw new AbuseOfPowerException();

            if (!IsOrganizationValid(perpetrator, user))
                throw new AmbigousOrganizationException();

            _repository.UserRepository.DeleteUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        private bool IsPerperatorPowered(User perpetrator, User manipulatedUser)
        {
            int maxPerpetratorPower = perpetrator.Roles.Max(r => r.RolePriority);
            int maxManipulatedUserPower = manipulatedUser.Roles.Max(r => r.RolePriority);

            return maxManipulatedUserPower < maxPerpetratorPower || IsUserInAdminRole(perpetrator);
        }

        private bool IsUserInAdminRole(User user)
            => user.Roles.FirstOrDefault(r => r.RoleName == "Admin") != null;

        private bool IsOrganizationValid(User perpetrator, User manipulatedUser)
        {
            bool perpetratorIsAdmin = IsUserInAdminRole(perpetrator);
            bool manipulatedUserIsAdmin = IsUserInAdminRole(manipulatedUser);

            bool adminManipulatesAdmin = perpetrator.Organization == null && manipulatedUser.Organization == null && perpetratorIsAdmin && manipulatedUserIsAdmin;
            bool userManipulatesAnotherUser = perpetrator.Organization != null && manipulatedUser.Organization != null && manipulatedUser.Organization.Id == perpetrator.Organization.Id;
            bool adminManipulatesAnotherUser = perpetratorIsAdmin && manipulatedUser.Organization != null;

            return adminManipulatesAdmin || userManipulatesAnotherUser || adminManipulatesAnotherUser;
        }

        private async Task<User> MapFromCreateUserDtoAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
            };
            
            user.Roles = await _repository.RoleRepository.MapRoleListAsync(createUserDto.UserRoles);

            return user;
        }

        private async Task MapFromUpdateUserDto(UpdateUserDto updateUserDto, User user)
        {
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Roles = await _repository.RoleRepository.MapRoleListAsync(updateUserDto.UserRoles);
        }

        private UserDto MapFromUserToDto(User user)
        {
            var userDto = new UserDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return userDto;
        }
    }
}
