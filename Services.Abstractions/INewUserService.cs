using Marc2.Contracts.Auth;
using Marc2.Contracts.User;
using System.Security.Claims;

namespace Marc2.Services.Abstractions
{
    public interface INewUserService
    {
        Task<IEnumerable<UserDto>> GetUsersByOganizationAsync(string perpetratorEmail, int organizationId = 0);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string perpetratorEmail, int organizationId = 0);
        Task UpdateUserAsync(UpdateUserDto updateUserDto, string perpetratorEmail, int userId);
        Task DeleteUserAsync(int userId, string perpetratorEmail);
        Task<IEnumerable<Claim>> GetUserClaimsAsync(UserAuthDto userAuthDto);
        Task<UserDto> GetSelfDataAsync(string userEmail);
        Task<bool> IsUserRefreshTokenValidAsync(string email, string refreshToken);
        Task UpdateUserRefreshTokenAsync(string? newRefreshToken, string email, DateTime? newExpiry);
        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, string userEmail);
        Task<UserDto> GetUserByEmailAsync(string userEmail);
    }
}
