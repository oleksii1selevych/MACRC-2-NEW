namespace Marc2.Services.Abstractions
{
    public interface IServiceManager
    {
        INewUserService UserService { get; }
        IOrganizationService OrganizationService { get; }
        IAccidentService AccidentService { get; }
        IRoleService RoleService { get; }
        ISmartBraceletService SmartBraceletService { get; }
        IWorkShiftService WorkShiftService { get; }
        IIssueService IssueService { get; }
    }
}
