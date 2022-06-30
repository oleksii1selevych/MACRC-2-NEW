using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Marc2.Persistence.Repositories
{
    public class SmartBraceletRepository : RepositoryBase<SmartBracelet>, ISmartBraceletRepository
    {

        public SmartBraceletRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CreateSmartBracelet(SmartBracelet smartBracelet)
            => Create(smartBracelet);

        public void DeleteSmartBracelet(SmartBracelet smartBracelet)
            => Delete(smartBracelet);

        public async Task<IEnumerable<SmartBracelet>> GetAllSmartBraceletsByOrganizationAsync(int organizationId)
            => await FindByCondition(s => s.OrganizationId == organizationId).AsNoTracking().ToListAsync();

        public async Task<IEnumerable<SmartBracelet>> GetLostConnectionSmartBracelets(int organizationId, int maxMinutesDelta)
        {
            var smartBracelets = new List<SmartBracelet>();
            var activeBracelets = await FindByCondition(s => s.LastRequest != null && s.IsActive && s.OrganizationId == organizationId).ToListAsync();
            foreach(var bracelet in activeBracelets)
            {
                var timeSpan = DateTime.Now - (DateTime)bracelet.LastRequest;
                if(timeSpan.TotalMinutes > maxMinutesDelta)
                    smartBracelets.Add(bracelet);
            }
            return smartBracelets;
        }            

        public async Task<SmartBracelet?> GetSmartBraceletByCodeAsync(string manufacturerCode)
               => await FindByCondition(s => s.ManufacturerCode == manufacturerCode).FirstOrDefaultAsync();
        public async Task<SmartBracelet?> GetSmartBraceletByIdAsync(int smartBraceletId)
            => await FindByCondition(s => s.Id == smartBraceletId).FirstOrDefaultAsync();
        
        public void UpdateSmartBracelet(SmartBracelet smartBracelet)
            => Update(smartBracelet);
    }
}
