using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class WorkShiftRepository : RepositoryBase<WorkShift>, IWorkShiftRepository
    {

        public WorkShiftRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CreateWorkShift(WorkShift workShift)
            => Create(workShift);

        public async Task<IEnumerable<WorkShift>> GetAllAvailibleWorkShiftsByAccident(int accidentId, int organizationId)
        {
            var workShifts = await FindByCondition(w => (w.EndedAt == null && w.User.Organization.Id == organizationId && w.Assignments.Count == 0) ||
            (w.Assignments.Any(a => a.Issue.AccidentId == accidentId) && w.User.Organization.Id == organizationId && w.EndedAt == null)
            || (w.User.Organization.Id == organizationId && w.EndedAt == null && w.Assignments.All(a => a.Issue.AccidentId != accidentId && a.Issue.IsCompleted)))
                .Include(w => w.SmartBracelet).Include(w => w.User).ToListAsync();
            return workShifts;
        }

        public async Task<IEnumerable<WorkShift>> GetAllWorkShiftsByAccident(int accidentId)
            => await FindByCondition(w => w.Assignments.Any(a => a.Issue.AccidentId == accidentId) && w.EndedAt == null)
            .Include(w => w.SmartBracelet).Include(w => w.User).ToListAsync();

        public async Task<IEnumerable<WorkShift>> GetAllWorkShiftsByUserIdAsync(int userId)
            => await FindByCondition(w => w.UserId == userId).ToListAsync();

        public async Task<WorkShift> GetUncompletedWorkShiftAsync(int userId)
            => await FindByCondition(w => w.EndedAt == null && w.UserId == userId).FirstAsync();

        public async Task<WorkShift?> GetWorkShiftByIdAsync(int workShiftId)
            => await FindByCondition(w => w.Id == workShiftId).FirstOrDefaultAsync();

        public async Task<WorkShift?> GetWorkShiftBySmartBraceletidAsync(int smartBraceletId)
            => await FindByCondition(w => w.EndedAt == null && w.SmartBraceletId == smartBraceletId).Include(w => w.User).FirstOrDefaultAsync();

        public async Task<bool> IsWorkShiftBelongedToAnotherAccident(int accidentId, int workShiftId)
            => await FindByCondition(w => w.Id == workShiftId && !w.Assignments.All(a => a.Issue.AccidentId == accidentId))
            .FirstOrDefaultAsync() != null;

        public void UpdateWorkShift(WorkShift workShift)
            => Update(workShift);
    }
}
