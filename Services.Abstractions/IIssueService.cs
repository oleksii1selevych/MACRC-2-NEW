using Marc2.Contracts.Issue;

namespace Marc2.Services.Abstractions
{
    public interface IIssueService
    {
        Task<IssueDto> CreateIssueAsync(CreateIssueDto createIssueDto, string userEmail);
        Task UpdateIssueAsync(UpdateIssueDto updateIssueDto, string userEmail, int issueId);
        Task DeleteIssueAsync(int issueId, string userEmail);
        Task<IEnumerable<IssueDto>> GetAllIssuesByAccident(int accidentId, string userEmail);
        Task ChangeIssueStatusAsync(int issueId, bool completenessStatus, string userEmail);
        Task<IEnumerable<ResquerIssueDto>> GetResquerIssuesAsync(string userEmail);
    }
}
