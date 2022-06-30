namespace Marc2.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        IRoleRepository RoleRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IAccidentRepository AccidentRepository { get; }
        ISmartBraceletRepository SmartBraceletRepository { get; }
        IWorkShiftRepository WorkShiftRepository { get; }
        IIssueRepository IssueRepository { get; }
        IAssignmentRepository AssignmentRepository { get; }
    }
}
