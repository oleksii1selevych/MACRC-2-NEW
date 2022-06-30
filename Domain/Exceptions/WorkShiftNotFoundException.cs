namespace Marc2.Domain.Exceptions
{
    public class WorkShiftNotFoundException : NotFoundException
    {
        public WorkShiftNotFoundException(int workShiftId) : base(String.Format("Workshift with id {0} does not exist", workShiftId))
        {
        }
    }
}
