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
    public class NewUserService : INewUserService
    {
        private readonly IRepositoryManager _repository;
        public NewUserService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByOganizationAsync(string perpetratorEmail, int organizationId = 0)
        {
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            if (!IsOrganizationValid(perpetrator, organizationId))
                throw new AmbigousOrganizationException();

            if(organizationId != 0)
            {
                var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
                if (organization == null)
                    throw new OrganizationNotFoundException(organizationId);
            }   

            var users = await _repository.UserRepository.GetUsersByOrganizationAsync(organizationId, perpetrator.Id);
            var dtos = users.Select(u => MapFromUserToDto(u)).AsEnumerable();
            return dtos;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string perpetratorEmail, int organizationId = 0)
        {
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            var user = await MapFromCreateUserDtoAsync(createUserDto);

            if (!IsPerperatorPowered(perpetrator, user))
                throw new AbuseOfPowerException();

            if (!IsOrganizationValid(perpetrator, organizationId))
                throw new AmbigousOrganizationException();

            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);

            if (organizationId != 0 && organization == null)
                throw new OrganizationNotFoundException(organizationId);

            user.Organization = organization;


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

        private bool IsPerperatorPowered(User perpetrator, User manipulatedUser)
        {
            int maxPerpetratorPower = perpetrator.Roles.Max(r => r.RolePriority);
            int maxManipulatedUserPower = manipulatedUser.Roles.Max(r => r.RolePriority);

            return maxManipulatedUserPower <= maxPerpetratorPower || IsUserInAdminRole(perpetrator);
        }

        private bool IsOrganizationValid(User perpetrator, int organizationId)
            => IsUserInAdminRole(perpetrator) || perpetrator.Organization.Id == organizationId;


        private bool IsUserInAdminRole(User user)
          => user.Roles.FirstOrDefault(r => r.RoleName == "Admin") != null;

        private UserDto MapFromUserToDto(User user)
        {
            var userDto = new UserDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrganizationId = user.Organization != null ? user.Organization.Id : 0,
                Roles = user.Roles.Select(r => new RoleDto { Id = r.Id, RolePriority = r.RolePriority, Name = r.RoleName})
            };
            return userDto;
        }

        private async Task<User> MapFromCreateUserDtoAsync(CreateUserDto createUserDto)
        {
            var user = new User()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber
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

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(UserAuthDto userAuthDto)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userAuthDto.Email);
            if (user == null)
                throw new InvalidUserCredentialsException();

            if (!PasswordHelper.CheckPassword(user.HashedPassword, userAuthDto.Password))
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

        public async Task<UserDto> GetSelfDataAsync(string userEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail) ?? null!;
            return MapFromUserToDto(user);
        }

        public async Task<bool> IsUserRefreshTokenValidAsync(string email, string refreshToken)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException(email);

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                return false;

            return true;
        }

        public async Task UpdateUserRefreshTokenAsync(string? newRefreshToken, string email, DateTime? newExpiry)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException(email);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = newExpiry != null ? newExpiry : user.RefreshTokenExpiry;

            _repository.UserRepository.UpdateUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();
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

            if (!IsOrganizationValid(perpetrator, user.Organization == null ? 0 : user.Organization.Id))
                throw new AmbigousOrganizationException();

            _repository.UserRepository.UpdateUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();

            await UpdateUserRefreshTokenAsync(null, user.Email, DateTime.Now.AddMinutes(-10));
        }

        public async Task DeleteUserAsync(int userId, string perpetratorEmail)
        {
            var user = await _repository.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(userId);

            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            if (!IsPerperatorPowered(perpetrator, user))
                throw new AbuseOfPowerException();

            if (!IsOrganizationValid(perpetrator, user.Organization == null ? 0 : user.Organization.Id))
                throw new AmbigousOrganizationException();

            _repository.UserRepository.DeleteUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, string userEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail);
            if (user == null)
                throw new UserNotFoundException(userEmail);

            if (!PasswordHelper.CheckPassword(user.HashedPassword, changePasswordDto.OldPassword))
                throw new InvalidUserCredentialsException();

            string passwordHash = PasswordHelper.Hash(changePasswordDto.NewPassword);
            user.HashedPassword = passwordHash;

            _repository.UserRepository.UpdateUser(user);
            await _repository.UnitOfWork.SaveChangesAsync();

            await UpdateUserRefreshTokenAsync(null, userEmail, DateTime.Now.AddMinutes(-10));
        }

        public async Task<UserDto> GetUserByEmailAsync(string userEmail)
            => MapFromUserToDto(await _repository.UserRepository.GetByEmailAsync(userEmail) ?? null!);
    }
}
