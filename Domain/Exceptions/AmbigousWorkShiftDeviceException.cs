namespace Marc2.Domain.Exceptions
{
    public class AmbigousWorkShiftDeviceException : BadRequestException
    {
        public AmbigousWorkShiftDeviceException() : base("Workshift does not belong to requested device")
        {
        }
    }
}
