using Marc2.Contracts.Auth;
using Marc2.Contracts.Role;
using Marc2.Contracts.User;
using System.Security.Claims;

namespace Marc2.Services.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string perpetratorEmail, int organizationId = 0);

        Task UpdateUserAsync(UpdateUserDto updateUserDto, string perpetratorEmail, int userId);
        Task DeleteUserAsync(int userId, string perpetratorEmail);

        Task<IEnumerable<UserDto>> GetAllUsersByOrganizationAsync(string perpetratorEmail, int organizationId = 0);

        Task<IEnumerable<Claim>> GetUserClaimsAsync(UserAuthDto userAuthDto);
    }
}
