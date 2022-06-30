namespace Marc2.Domain.Exceptions
{
    public class DeviceNotFoundException : NotFoundException
    {
        public DeviceNotFoundException(int deviceId) : base(String.Format("Smart device with id {0} does not exist", deviceId))
        {
        }

        public DeviceNotFoundException(string manufacturerCode) : base(String.Format("Smart device with manufacturer code {0} does not exist", manufacturerCode))
        {
        }
    }
}
