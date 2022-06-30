namespace Marc2.Domain.Exceptions
{
    public class OrganizationNotFoundException : NotFoundException
    {
        public OrganizationNotFoundException(int organizationId)
          : base(String.Format("Organization with id {0} does not exist", organizationId)) { }
    }
}
