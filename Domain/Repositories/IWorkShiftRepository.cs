using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IWorkShiftRepository
    {
        Task<IEnumerable<WorkShift>> GetAllWorkShiftsByUserIdAsync(int userId);
        void CreateWorkShift(WorkShift workShift);
        void UpdateWorkShift(WorkShift workShift);
        Task<WorkShift> GetUncompletedWorkShiftAsync(int userId);
        Task<IEnumerable<WorkShift>> GetAllAvailibleWorkShiftsByAccident(int accidentId, int organizationId);
        Task<IEnumerable<WorkShift>> GetAllWorkShiftsByAccident(int accidentId);
        Task<WorkShift?> GetWorkShiftByIdAsync(int workShiftId);
        Task<bool> IsWorkShiftBelongedToAnotherAccident(int accidentId, int workShiftId);
        Task<WorkShift?> GetWorkShiftBySmartBraceletidAsync(int smartBraceletId);
    }
}
