using System.Security.Claims;

namespace Marc2.Tokens
{
    public interface ITokenService 
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expiresAt);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
