namespace Marc2.Domain.Exceptions
{
    public class AllWorkShiftsCompletedException : BadRequestException
    {
        public AllWorkShiftsCompletedException() : base("Try to complete work shift when all of them are completed")
        {
        }
    }
}
