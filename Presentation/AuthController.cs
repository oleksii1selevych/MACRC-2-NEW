using Marc2.Contracts.Auth;
using Marc2.Services.Abstractions;
using Marc2.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IServiceManager _services;

        public AuthController(ITokenService tokenService, IServiceManager services)
        {
            _tokenService = tokenService;
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserAuthDto userAuthDto)
        {
            var claims = await _services.UserService.GetUserClaimsAsync(userAuthDto);

            int accessTokenExpirationMin = 25;
            int refreshTokenExpiratonMin = 240;


            DateTime refreshTokenExpiresAt = DateTime.Now.AddMinutes(refreshTokenExpiratonMin);
            DateTime accessTokenExpiresAt = DateTime.Now.AddMinutes(accessTokenExpirationMin);

            var accessToken = _tokenService.GenerateAccessToken(claims, accessTokenExpiresAt);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _services.UserService.UpdateUserRefreshTokenAsync(refreshToken, userAuthDto.Email, refreshTokenExpiresAt);
            return Ok(new TokenPairDto { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresAfter = accessTokenExpirationMin * 60});
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens([FromBody] TokenPairDto tokenPair)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenPair.AccessToken);
            string email = principal.Claims.First(c => c.Type == ClaimTypes.Email).Value;

            if (await _services.UserService.IsUserRefreshTokenValidAsync(email, tokenPair.RefreshToken))
            {
                int expirationMinutes = 25;
                DateTime expiresAt = DateTime.Now.AddSeconds(expirationMinutes);

                var newRefreshToken = _tokenService.GenerateRefreshToken();
                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims, expiresAt);

                await _services.UserService.UpdateUserRefreshTokenAsync(newRefreshToken, email, null);
                return Ok(new TokenPairDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken, ExpiresAfter = expirationMinutes * 60});
            }
         return BadRequest("Invalid refresh token specified");
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            string email = RetriveEmailFromHttpContext();
            await _services.UserService.UpdateUserRefreshTokenAsync(null, email, DateTime.Now.AddMinutes(-10));
            return NoContent();
        }

        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }
    }
}
