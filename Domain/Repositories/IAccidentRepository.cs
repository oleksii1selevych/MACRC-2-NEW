using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IAccidentRepository
    {
        void CreateAccident(Accident accident);
        void UpdateAccident(Accident accident);
        void DeleteAccident(Accident accident);

        Task<IEnumerable<Accident>> GetAccidentsByOrganizationAsync(int organizationId);
        Task<Accident?> GetAccidentById(int accidentId);
    }
}
