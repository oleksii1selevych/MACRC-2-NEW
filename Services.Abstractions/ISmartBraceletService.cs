using Marc2.Alert;
using Marc2.Contracts.SmartBracelets;

namespace Marc2.Services.Abstractions
{
    public interface ISmartBraceletService
    {
        Task<IEnumerable<SmartBraceletDto>> GetDevicesByOrganizationAsync(int organizationId);
        Task<SmartBraceletDto> CreateDeviceAsync(CreateSmartBraceletDto createSmartBraceletDto, string certificatePassword);
        Task<string> UpdateDeviceAsync(int smartBraceletId, string hostAddress, string newCertificatePassword);
        Task<string> DeleteDeviceAsync(int smartBraceletId);
        Task ChangeDeviceStatusAsync(bool deviceStatus, string manufacturerCode);

        Task<ResquerAlertDto?> ChangeDeviceDataAsync(UpdateBraceletDataDto updateBraceletDataDto, string manufacturerCode);
        Task<bool> IsExisintDeviceAsync(string manufacturerNumber);
        Task<ResquerAlertDto> GetEmergenceRequestDataAsync(string manufacturerCode);
        Task<IEnumerable<ResquerAlertDto>> GetLostConnectionDevices(string manufacturerCode);
    }
}
