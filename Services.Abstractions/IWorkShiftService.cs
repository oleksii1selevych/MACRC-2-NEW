using Marc2.Contracts.WorkShifts;

namespace Marc2.Services.Abstractions
{
    public interface IWorkShiftService
    {
        Task StartWorkShiftAsync(int smartBraceletId, string resquerEmail);
        Task EndWorkShiftAsync(string resquerEmail);
        Task<IEnumerable<WorkShiftDto>> GetAllAvailibleWorkShifts(int accidentId, string userEmail);
        Task<IEnumerable<WorkShiftDto>> GetAllWorkShiftsByAccident(int accidentId, string userEmail);
    }
}
