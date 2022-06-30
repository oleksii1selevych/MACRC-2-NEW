using Marc2.Contracts.Issue;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IServiceManager _services;
        public IssueController(IServiceManager serviceManager)
        {
            _services = serviceManager;
        }

        
        private string RetriveEmailFromHttpContext()
        {
            var emailClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIssuesByAccident(int accidentId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var issueDtos = await _services.IssueService.GetAllIssuesByAccident(accidentId, userEmail);
            return Ok(issueDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDto createIssueDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            var issue = await _services.IssueService.CreateIssueAsync(createIssueDto, userEmail);
            return Created("someLink", issue);
        }

        [HttpPut("{issueId:int}")]
        public async Task<IActionResult> UpdateIssue(int issueId, [FromBody] UpdateIssueDto updateIssueDto)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.IssueService.UpdateIssueAsync(updateIssueDto, userEmail, issueId);
            return NoContent();
        }

        [HttpPut("changeStatus/{issueId:int}")]
        public async Task<IActionResult> ChangeIssueStatus(int issueId, [FromBody] bool completenessStatus)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.IssueService.ChangeIssueStatusAsync(issueId, completenessStatus, userEmail);
            return NoContent();
        }

        [HttpDelete("{issueId:int}")]
        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            var userEmail = RetriveEmailFromHttpContext();
            await _services.IssueService.DeleteIssueAsync(issueId, userEmail);
            return NoContent();
        }
    }
}
