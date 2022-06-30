namespace Marc2.Domain.Exceptions
{
    public class AmbigousAccidentsIssuesException : BadRequestException
    {
        public AmbigousAccidentsIssuesException() : base("Resquer alreasy performs issues in another accident")
        {
        }
    }
}
