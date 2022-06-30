using Marc2.Alert;
using Marc2.CertificateIssuerService;
using Marc2.Contracts.SmartBracelets;
using Marc2.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marc2.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        private readonly ICertificateIssuer _certificateIssuer;
        private readonly IServiceManager _services;
        private readonly IAlertService _alertService;

        public IoTController(ICertificateIssuer certificateIssuer, IServiceManager services, IAlertService alertService)
        {
            _services = services;
            _certificateIssuer = certificateIssuer;
            _alertService = alertService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDevicesByOrganization(int organizationId)
        {
            var deviceDtos = await _services.SmartBraceletService.GetDevicesByOrganizationAsync(organizationId);
            return Ok(deviceDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDevice([FromBody] CreateSmartBraceletDto createSmartBraceletDto)
        {
            string certificatePassword = _certificateIssuer.GenerateRandomPassword();

            var smartBraceletDto = await _services.SmartBraceletService.CreateDeviceAsync(createSmartBraceletDto, certificatePassword);

            _certificateIssuer.IssueDeviceCertificate(createSmartBraceletDto.ManufacturerCode, certificatePassword, createSmartBraceletDto.HostingAddress);
            return Created("some url", smartBraceletDto);
        }


        [HttpPut("{smartBraceletId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDevice(int smartBraceletId, [FromBody] string deviceHost)
        {
            string certificatePassword = _certificateIssuer.GenerateRandomPassword();
            string manufacturerCode = await _services.SmartBraceletService.UpdateDeviceAsync(smartBraceletId, deviceHost, certificatePassword);

            _certificateIssuer.DeleteCertificate(manufacturerCode);
            _certificateIssuer.IssueDeviceCertificate(manufacturerCode, certificatePassword, deviceHost);
            return NoContent();
        }

        [HttpDelete("{smartBraceletId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDevice(int smartBraceletId)
        {
            string manufacturerCode = await _services.SmartBraceletService.DeleteDeviceAsync(smartBraceletId);
            _certificateIssuer.DeleteCertificate(manufacturerCode);
            return NoContent();
        }

        [HttpPut("changeStatus")]
        [Authorize(Roles = "IoT")]
        public async Task<IActionResult> ChangeDeviceStatus([FromBody] bool deviceStatus)
        {
            var manufacturerCode = RetrieveDeviceManufacturerCode();
            await _services.SmartBraceletService.ChangeDeviceStatusAsync(deviceStatus, manufacturerCode);
            return NoContent();
        }

        [HttpPut("changeData")]
        [Authorize(Roles = "IoT")]
        public async Task<IActionResult> ChangeDeviceData([FromBody] UpdateBraceletDataDto updateBraceletDataDto)
        {
            var manufacturerCode = RetrieveDeviceManufacturerCode();
            var resquerAlertDto = await _services.SmartBraceletService.ChangeDeviceDataAsync(updateBraceletDataDto, manufacturerCode);
            if (resquerAlertDto != null)
                _alertService.AddAlert(resquerAlertDto);

            var resquerAlertDtos = await _services.SmartBraceletService.GetLostConnectionDevices(manufacturerCode);
            foreach (var alertDto in resquerAlertDtos)
                _alertService.AddAlert(alertDto);

            return NoContent();
        }

        [HttpPost("emergence")]
        [Authorize(Roles = "IoT")]
        public async Task<IActionResult> SendEmergence()
        {
            var manufacturerCode = RetrieveDeviceManufacturerCode();
            var resquerAlertDto = await _services.SmartBraceletService.GetEmergenceRequestDataAsync(manufacturerCode);
            _alertService.AddAlert(resquerAlertDto);
            return NoContent();
        }

        [HttpGet("certificate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeviceCertificate(string manufacturerCode)
        {
            if (await _services.SmartBraceletService.IsExisintDeviceAsync(manufacturerCode)){
                byte[] bytes = _certificateIssuer.GetCertificate(manufacturerCode);
                return File(bytes, "application/octet-stream", String.Format("{0}.pfx", manufacturerCode));
            }
            return NoContent();
        }

        private string RetrieveDeviceManufacturerCode()
        {
            var manufacturerCodeClaim = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name);
            return manufacturerCodeClaim.Value.Substring(3);
        }
    }
}
