namespace Marc2.Domain.Exceptions
{
    public class AbuseOfPowerException : BadRequestException
    {
        public AbuseOfPowerException()
            : base("You don't have enough power do perform this action") { }
    }
}
