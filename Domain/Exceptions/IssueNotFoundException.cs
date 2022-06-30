namespace Marc2.Domain.Exceptions
{
    public class IssueNotFoundException : NotFoundException
    {
        public IssueNotFoundException(int issueId) : base(String.Format("Issue with id {0} does not exist", issueId))
        {
        }
    }
}
