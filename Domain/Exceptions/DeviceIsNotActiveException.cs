namespace Marc2.Domain.Exceptions
{
    public class DeviceIsNotActiveException : BadRequestException
    {
        public DeviceIsNotActiveException(string manufacturerCode) : base(String.Format("Device with code {0} is currently not active", manufacturerCode))
        {
        }
    }
}
