using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IIssueRepository
    {
        Task<Issue?> GetIssueByIdAsync(int issueId);
        void DeleteIssue(Issue issue);
        void UpdateIssue(Issue issue);
        void CreateIssue(Issue issue);
        Task<IEnumerable<Issue>> GetAllIssuesByIdAccidentAsync(int accidentId);
    }
}
