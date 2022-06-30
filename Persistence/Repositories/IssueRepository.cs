using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
    {
        public IssueRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void CreateIssue(Issue issue)
            => Create(issue);

        public void DeleteIssue(Issue issue)
            => Delete(issue);

        public async Task<IEnumerable<Issue>> GetAllIssuesByIdAccidentAsync(int accidentId)
            => await FindByCondition(i => i.AccidentId == accidentId).Include(i => i.Assignments).ToListAsync();

        public async Task<Issue?> GetIssueByIdAsync(int issueId)
            => await FindByCondition(i => i.Id == issueId).Include(i => i.Assignments).Include(i=>i.Accident).FirstOrDefaultAsync();

        public void UpdateIssue(Issue issue)
            => Update(issue);
    }
}
