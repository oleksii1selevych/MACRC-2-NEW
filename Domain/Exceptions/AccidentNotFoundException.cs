namespace Marc2.Domain.Exceptions
{
    public class AccidentNotFoundException : NotFoundException
    {
        public AccidentNotFoundException(int accidentId)
            : base(String.Format("Accident with id {0} does not exist", accidentId))
        {
        }
    }
}
