using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IServiceManager _services;

        public RoleController(IServiceManager services)
            => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetUserRoles()
        {

            var userEmail = RetriveEmailFromHttpContext();
            var roleDtos = await _services.RoleService.GetUserRolesAsync(userEmail);

            return Ok(roleDtos);
        }

        [HttpGet("allRoles")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetPossibleRoles()
        {
            var userEmail = RetriveEmailFromHttpContext();
            var roleDtos = await _services.RoleService.GetAllPossibleRolesAsync(userEmail);
            return Ok(roleDtos);
        }

        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }
    }
}
