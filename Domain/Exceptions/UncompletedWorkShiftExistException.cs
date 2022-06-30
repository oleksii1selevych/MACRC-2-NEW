namespace Marc2.Domain.Exceptions
{
    public class UncompletedWorkShiftExistException : BadRequestException
    {
        public UncompletedWorkShiftExistException() : base(String.Format("Try to start a new work shift with an existing one"))
        {
        }
    }
}
