namespace Marc2.Domain.Exceptions
{
    public class AmbigousOrganizationException : BadRequestException
    {
        public AmbigousOrganizationException()
            : base(String.Format("Attempt on perfoming actions on user that does not belong to your organization")) { }
    }
}
