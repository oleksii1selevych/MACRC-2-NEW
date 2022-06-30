using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface ISmartBraceletRepository
    {
        Task<IEnumerable<SmartBracelet>> GetAllSmartBraceletsByOrganizationAsync(int organizationId);
        Task<SmartBracelet?> GetSmartBraceletByIdAsync(int smartBraceletId);
        Task<SmartBracelet?> GetSmartBraceletByCodeAsync(string manufacturerCode);
        void CreateSmartBracelet(SmartBracelet smartBracelet);
        void DeleteSmartBracelet(SmartBracelet smartBracelet);
        void UpdateSmartBracelet(SmartBracelet smartBracelet);
        Task<IEnumerable<SmartBracelet>> GetLostConnectionSmartBracelets(int organizationId, int maxMinutesDelta); 
    }
}
