namespace Marc2.Domain.Exceptions
{
    public class InvalidAccidentOrganizationException : BadRequestException
    {
        public InvalidAccidentOrganizationException() : base("Try to retrieve accident information from not your ogranization")
        {
        }

    }
}
