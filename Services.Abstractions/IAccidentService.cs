using Marc2.Contracts.Accident;

namespace Marc2.Services.Abstractions
{
    public interface IAccidentService
    {
        Task<AccidentDto> CreateAccidentAsync(CreateAccidentDto createAccidentDto, string perpetratorEmail);
        Task UpdateAccidentAsync(int accidentId, UpdateAccidentDto updateAccidentDto, string perpetratorEmail);
        Task DeleteAccidentAsync(int accidentId, string perpetratorEmail);
        Task<AccidentDto> GetAccidentByIdAsync(int accidentId, string perpetratorEmail);
        Task<IEnumerable<AccidentDto>> GetAllAccidentsByOrganizationAsync(string perpetratorEmail);
    }
}
