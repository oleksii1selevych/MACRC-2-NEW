using Marc2.Contracts.Organization;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IServiceManager _services;
        public OrganizationController(IServiceManager serviceManager)
        {
            _services = serviceManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var organizationDtos = await _services.OrganizationService.GetAllOrganizationsAsync();
            return Ok(organizationDtos);
        }

        [HttpGet("{organizationId:int}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetOrganizationById(int organizationId)
        {
            var organizationDto = await _services.OrganizationService.GetOrganizationByIdAsync(organizationId);
            return Ok(organizationDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationDto createOrganizationDto)
        {
            var organizationDto = await _services.OrganizationService.CreateOrganizationAsync(createOrganizationDto);
            return CreatedAtAction(nameof(GetOrganizationById), new { organizationId = organizationDto.OrganizationId }, organizationDto);
        }

        [HttpPut("{organizationId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrganization(int organizationId, [FromBody] UpdateOrganizationDto updateOrganizationDto)
        {
            await _services.OrganizationService.UpdateOrganizationAsync(organizationId, updateOrganizationDto);
            return NoContent();
        }

        [HttpDelete("{organizationId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrganization(int organizationId)
        {
            await _services.OrganizationService.DeleteOrganizationAsync(organizationId);
            return NoContent();
        }
    }
}
