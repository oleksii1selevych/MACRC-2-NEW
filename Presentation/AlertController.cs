using Marc2.Alert;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Resquer, Dispatcher")]
    public class AlertController : ControllerBase
    {
        private readonly IServiceManager _services;
        private readonly IAlertService _alertService;
        public AlertController(IServiceManager serviceManager, IAlertService alertService)
        {
            _services = serviceManager;
            _alertService = alertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var userEmail = RetriveEmailFromHttpContext();
            var userDto = await _services.UserService.GetUserByEmailAsync(userEmail);

            var alerts = _alertService.GetAlerts(userDto.Email, userDto.OrganizationId);
            return Ok(alerts);
        }
        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }
    }
}
