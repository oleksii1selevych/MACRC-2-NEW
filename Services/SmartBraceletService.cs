using Marc2.Alert;
using Marc2.Contracts.SmartBracelets;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class SmartBraceletService : ISmartBraceletService
    {
        private readonly IRepositoryManager _repository;
        private const int pulseDelta = 100;
        private const double spo2Delta = 8;
        private const int maxMinutesDelta = 5;
        public SmartBraceletService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<SmartBraceletDto> CreateDeviceAsync(CreateSmartBraceletDto createSmartBraceletDto, string certificatePassword)
        {
            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(createSmartBraceletDto.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(createSmartBraceletDto.OrganizationId);

            var smartBracelet = MapFromCreateSmartBraceletDto(createSmartBraceletDto, certificatePassword);

            _repository.SmartBraceletRepository.CreateSmartBracelet(smartBracelet);
            await _repository.UnitOfWork.SaveChangesAsync();

            return MapToSmartBraceletToDto(smartBracelet);
        }

        private SmartBracelet MapFromCreateSmartBraceletDto(CreateSmartBraceletDto createSmartBraceletDto, string certificatePassword)
        {
            var smartBracelet = new SmartBracelet
            {
                ManufacturerCode = createSmartBraceletDto.ManufacturerCode,
                CertificatePassword = certificatePassword,
                OrganizationId = createSmartBraceletDto.OrganizationId,
                HostAddress = createSmartBraceletDto.HostingAddress
            };

            return smartBracelet;
        }

        private void MapFromUpdateBraceletDataDto(UpdateBraceletDataDto updateBraceletDataDto, SmartBracelet smartBracelet)
        {
            smartBracelet.Lattitude = updateBraceletDataDto.Lattitude;
            smartBracelet.Logngtitude = updateBraceletDataDto.Longtitude;
            smartBracelet.PulseRate = updateBraceletDataDto.PulseRate;
            smartBracelet.Spo2Percentage = updateBraceletDataDto.Spo2Percentage;
            smartBracelet.LastRequest = DateTime.Now;
        }

        private SmartBraceletDto MapToSmartBraceletToDto(SmartBracelet smartBracelet)
        {
            var dto = new SmartBraceletDto
            {
                IsActive = smartBracelet.IsActive,
                CertificatePassword = smartBracelet.CertificatePassword,
                HostingAddress = smartBracelet.HostAddress,
                ManufacturerCode = smartBracelet.ManufacturerCode,
                OrganizationId = smartBracelet.OrganizationId,
                SmartBraceletId = smartBracelet.Id
            };

            return dto;
        }
        public async Task<string> DeleteDeviceAsync(int smartBraceletId)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByIdAsync(smartBraceletId);
            if (smartDevice == null)
                throw new DeviceNotFoundException(smartBraceletId);

            _repository.SmartBraceletRepository.DeleteSmartBracelet(smartDevice);
            await _repository.UnitOfWork.SaveChangesAsync();

            return smartDevice.ManufacturerCode;
        }

        public async Task<IEnumerable<SmartBraceletDto>> GetDevicesByOrganizationAsync(int organizationId)
        {
            var organization = await _repository.OrganizationRepository.GetOrganizationByIdAsync(organizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(organizationId);

            var smartBracelets = await _repository.SmartBraceletRepository.GetAllSmartBraceletsByOrganizationAsync(organizationId);
            return smartBracelets.Select(s => MapToSmartBraceletToDto(s)).ToList();
        }

        public async Task<string> UpdateDeviceAsync(int smartBraceletId, string hostAddress, string newCertificatePassword)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByIdAsync(smartBraceletId);
            if (smartDevice == null)
                throw new DeviceNotFoundException(smartBraceletId);

            smartDevice.HostAddress = hostAddress;
            smartDevice.CertificatePassword = newCertificatePassword;
           
            _repository.SmartBraceletRepository.UpdateSmartBracelet(smartDevice);
            await _repository.UnitOfWork.SaveChangesAsync();

            return smartDevice.ManufacturerCode;
        }

        public async Task ChangeDeviceStatusAsync(bool deviceStatus, string manufacturerCode)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByCodeAsync(manufacturerCode);
            if (smartDevice == null)
                throw new DeviceNotFoundException(manufacturerCode);

            smartDevice.IsActive = deviceStatus;

            _repository.SmartBraceletRepository.UpdateSmartBracelet(smartDevice);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task<ResquerAlertDto?> ChangeDeviceDataAsync(UpdateBraceletDataDto updateBraceletDataDto, string manufacturerCode)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByCodeAsync(manufacturerCode);
            if (smartDevice == null)
                throw new DeviceNotFoundException(manufacturerCode);

            if (!smartDevice.IsActive)
                throw new DeviceIsNotActiveException(manufacturerCode);

            ResquerAlertDto resquerAlertDto = null;

            int currentPulseDelta = Math.Abs(updateBraceletDataDto.PulseRate - smartDevice.PulseRate == null ? 0 : (int)smartDevice.PulseRate);
            double currentSpo2Delta = Math.Abs(updateBraceletDataDto.Spo2Percentage - smartDevice.Spo2Percentage == null ? 0 : (double)smartDevice.Spo2Percentage);

            if((currentPulseDelta >= pulseDelta && 
                smartDevice.PulseRate != null && smartDevice.PulseRate != 0) ||
                (currentSpo2Delta >= spo2Delta && 
                smartDevice.Spo2Percentage != null && 
                smartDevice.Spo2Percentage != 0))
            {
                MapFromUpdateBraceletDataDto(updateBraceletDataDto, smartDevice);
                resquerAlertDto = await InitializeResquerAlertDto(smartDevice);
                resquerAlertDto.AlertType = AlertTypes.State;
            }
            else
            {
                MapFromUpdateBraceletDataDto(updateBraceletDataDto, smartDevice);
            }

            _repository.SmartBraceletRepository.UpdateSmartBracelet(smartDevice);
            await _repository.UnitOfWork.SaveChangesAsync();

            return resquerAlertDto;
        }

        public async Task<bool> IsExisintDeviceAsync(string manufacturerCode)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByCodeAsync(manufacturerCode);
            if (smartDevice == null)
                throw new DeviceNotFoundException(manufacturerCode);
            return true;
        }

        private async Task<ResquerAlertDto> InitializeResquerAlertDto(SmartBracelet smartBracelet)
        {
            var resquerAlertDto = new ResquerAlertDto
            {
                PulseRate = smartBracelet.PulseRate == null ? 0 : (int)smartBracelet.PulseRate,
                Spo2Percentage = smartBracelet.Spo2Percentage == null ? 0 : (double)smartBracelet.Spo2Percentage,
                Lattitude = smartBracelet.Lattitude == null ? 0 : (double)smartBracelet.Lattitude,
                Longtitude = smartBracelet.Logngtitude == null ? 0 : (double)smartBracelet.Logngtitude,
                OrganizationId = smartBracelet.OrganizationId,
            };
            var workShift = await _repository.WorkShiftRepository.GetWorkShiftBySmartBraceletidAsync(smartBracelet.Id);
            if (workShift == null)
                throw new AmbigousWorkShiftDeviceException();
            resquerAlertDto.ResquerFullName = String.Format("{0} {1}", workShift.User.FirstName, workShift.User.LastName);
            resquerAlertDto.WorkShiftId = workShift.Id;
            return resquerAlertDto;
        }


        public async Task<ResquerAlertDto> GetEmergenceRequestDataAsync(string manufacturerCode)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByCodeAsync(manufacturerCode);
            if (smartDevice == null)
                throw new DeviceNotFoundException(manufacturerCode);

            var resquerAlertDto = await InitializeResquerAlertDto(smartDevice);
            resquerAlertDto.AlertType = AlertTypes.Emergence;
           
            return resquerAlertDto;
        }

        public async Task<IEnumerable<ResquerAlertDto>> GetLostConnectionDevices(string manufacturerCode)
        {
            var smartDevice = await _repository.SmartBraceletRepository.GetSmartBraceletByCodeAsync(manufacturerCode);
            if (smartDevice == null)
                throw new DeviceNotFoundException(manufacturerCode);

            var smartBracelets = await _repository.SmartBraceletRepository.GetLostConnectionSmartBracelets(smartDevice.OrganizationId, maxMinutesDelta);
            var resquerAlertDtos = new List<ResquerAlertDto>();
            foreach(var s in smartBracelets)
            {
                var resquerAlertDto = await InitializeResquerAlertDto(s);
                resquerAlertDto.AlertType = AlertTypes.NoConnection;
            }
            return resquerAlertDtos;
        }
    }
}
