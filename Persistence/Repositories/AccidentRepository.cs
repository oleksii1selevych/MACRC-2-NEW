using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class AccidentRepository : RepositoryBase<Accident>, IAccidentRepository
    {
        public AccidentRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void CreateAccident(Accident accident)
            => Create(accident);
        public void DeleteAccident(Accident accident)
            => Delete(accident);
        public async Task<Accident?> GetAccidentById(int accidentId)
            => await FindByCondition(a => a.Id == accidentId).Include(a => a.Organization).FirstOrDefaultAsync();
        public async Task<IEnumerable<Accident>> GetAccidentsByOrganizationAsync(int organizationId)
            => await FindByCondition(a => a.OrganizationId == organizationId).ToListAsync();

        public void UpdateAccident(Accident accident)
            => Update(accident);
    }
}
