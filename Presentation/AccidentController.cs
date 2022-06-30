using Marc2.Contracts.Accident;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Dispatcher")]
    public class AccidentController : ControllerBase
    {
        private readonly IServiceManager _services;
        public AccidentController(IServiceManager serviceManager)
        {
            _services = serviceManager;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccident([FromBody] CreateAccidentDto createAccidentDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var accidentDto = await _services.AccidentService.CreateAccidentAsync(createAccidentDto, userEmail);
            return Created("ff", accidentDto);
        }

        [HttpPut("{accidentId:int}")]
        public async Task<IActionResult> UpdateAccident(int accidentId, [FromBody] UpdateAccidentDto updateAccidentDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.AccidentService.UpdateAccidentAsync(accidentId, updateAccidentDto, userEmail);
            return NoContent();
        }

        [HttpDelete("{accidentId:int}")]
        public async Task<IActionResult> DeleteAccident(int accidentId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.AccidentService.DeleteAccidentAsync(accidentId, userEmail);
            return NoContent();
        }

        [HttpGet("{accidentId:int}")]
        public async Task<IActionResult> GetAccidentById(int accidentId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var accidentDto = await _services.AccidentService.GetAccidentByIdAsync(accidentId, userEmail);
            return Ok(accidentDto);
        }
        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAccidents()
        {
            var userEmail = RetriveEmailFromHttpContext();
            var accidentDtos = await _services.AccidentService.GetAllAccidentsByOrganizationAsync(userEmail);
            return Ok(accidentDtos);
        }
    }
}
