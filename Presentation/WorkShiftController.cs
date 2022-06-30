using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkShiftController : ControllerBase
    {
        private readonly IServiceManager _services;
        public WorkShiftController(IServiceManager serviceManager)
        {
            _services = serviceManager;
        }

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartWorkShift(int smartBraceletId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.WorkShiftService.StartWorkShiftAsync(smartBraceletId, userEmail);
            return Ok();
        }

        [HttpPost("end")]
        [Authorize]
        public async Task<IActionResult> EndWorkShift()
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.WorkShiftService.EndWorkShiftAsync(userEmail);
            return Ok();
        }

        [HttpGet("allAvailible")]
        public async Task<IActionResult> GetAllAvalibleWorkShifts(int accidentId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var workShiftDtos = await _services.WorkShiftService.GetAllAvailibleWorkShifts(accidentId, userEmail);
            return Ok(workShiftDtos);
        }
        [HttpGet("byAccident")]
        public async Task<IActionResult> GetAllWorkShiftsByAccident(int accidentId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var workShiftDtos = await _services.WorkShiftService.GetAllWorkShiftsByAccident(accidentId, userEmail);
            return Ok(workShiftDtos);
        }

        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }
    }
}
