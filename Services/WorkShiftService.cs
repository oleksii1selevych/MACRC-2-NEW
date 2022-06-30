using Marc2.Contracts.WorkShifts;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class WorkShiftService : IWorkShiftService
    {
        private readonly IRepositoryManager _repository;
        public WorkShiftService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task EndWorkShiftAsync(string resquerEmail)
        {
            var user = await _repository.UserRepository.GetByEmailAsync(resquerEmail) ?? null!;
            if (await AreAllWorkShiftsCompleted(user.Id))
                throw new AllWorkShiftsCompletedException();
            
            var workShift = await _repository.WorkShiftRepository.GetUncompletedWorkShiftAsync(user.Id);
            workShift.EndedAt = DateTime.Now;
            _repository.WorkShiftRepository.UpdateWorkShift(workShift);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkShiftDto>> GetAllAvailibleWorkShifts(int accidentId, string userEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail) ?? null!;

            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            if (accident.Organization.Id != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            var workShifts = await _repository.WorkShiftRepository.GetAllAvailibleWorkShiftsByAccident(accidentId, user.Organization.Id);
            return workShifts.Select(w => MapFromWorkShiftToDto(w)).ToList();
        }

        public async Task<IEnumerable<WorkShiftDto>> GetAllWorkShiftsByAccident(int accidentId, string userEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            var user = await _repository.UserRepository.GetByEmailAsync(userEmail) ?? null!;

            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            if (accident.Organization.Id != user.Organization.Id)
                throw new InvalidAccidentOrganizationException();

            var workShifts = await _repository.WorkShiftRepository.GetAllWorkShiftsByAccident(accidentId);
            return workShifts.Select(w => MapFromWorkShiftToDto(w)).ToList();
        }

        private WorkShiftDto MapFromWorkShiftToDto(WorkShift workShift)
        {
            var workShiftDto = new WorkShiftDto
            {
                Lattitude = workShift.SmartBracelet.Lattitude == null ? 0 : (double)workShift.SmartBracelet.Lattitude,
                Longtitude = workShift.SmartBracelet.Logngtitude == null ? 0 : (double)workShift.SmartBracelet.Logngtitude,
                UserEmail = workShift.User.Email,
                UserFullName = String.Format("{0} {1}", workShift.User.FirstName, workShift.User.LastName),
                PulseRate = workShift.SmartBracelet.PulseRate == null ? 0 : (int)workShift.SmartBracelet.PulseRate,
                Spo2Percentage = workShift.SmartBracelet.Spo2Percentage == null ? 0 : (double)workShift.SmartBracelet.Spo2Percentage,
                WorkShiftId = workShift.Id
            };
            return workShiftDto;
        }

        public async Task StartWorkShiftAsync(int smartBraceletId, string resquerEmail)
        {
            var device = await _repository.SmartBraceletRepository.GetSmartBraceletByIdAsync(smartBraceletId);
            if (device == null)
                throw new DeviceNotFoundException(smartBraceletId);

            var user = await _repository.UserRepository.GetByEmailAsync(resquerEmail) ?? null!;
            if (! await AreAllWorkShiftsCompleted(user.Id))
                throw new UncompletedWorkShiftExistException();

            if (!device.IsActive)
                throw new DeviceIsNotActiveException(device.ManufacturerCode);

            var workShift = new WorkShift()
            {
                StartedAt = DateTime.Now,
                UserId = user.Id,
                SmartBraceletId = device.Id,
            };

            _repository.WorkShiftRepository.CreateWorkShift(workShift);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<bool> AreAllWorkShiftsCompleted(int userId)
        {
            var workShifts = await _repository.WorkShiftRepository.GetAllWorkShiftsByUserIdAsync(userId);
            bool allCompeleted = workShifts.Any(w => w.EndedAt == null) == false;
            return allCompeleted;
        }
    }
}
