using Marc2.Contracts.User;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _services;

        public UserController(IServiceManager serviceManager)
        {
            _services = serviceManager;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetAllUsers(int organizationId = 0)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var userDtos = await _services.UserService.GetUsersByOganizationAsync(userEmail, organizationId);
            return Ok(userDtos);
        }
        [Authorize]
        [HttpGet("selfData")]
        public async Task<IActionResult> GetUserSelfData()
        {
            var userEmail = RetriveEmailFromHttpContext();
            var userDto = await _services.UserService.GetSelfDataAsync(userEmail);
            return Ok(userDto);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto, int organizationId = 0)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var userDto = await _services.UserService.CreateUserAsync(createUserDto, userEmail, organizationId);
            return Created("some_link", userDto);
        }

        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }

        [HttpPut("{userId:int}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.UserService.UpdateUserAsync(updateUserDto, userEmail, userId);
            return NoContent();
        }

        [HttpDelete("{userId:int}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.UserService.DeleteUserAsync(userId, userEmail);
            return NoContent();
        }

        [HttpPut("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.UserService.ChangePasswordAsync(changePasswordDto, userEmail);
            return NoContent();
        }
    }
}
