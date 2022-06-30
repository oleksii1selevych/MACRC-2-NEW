using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<INewUserService> _lazyNewUserService;
        private readonly Lazy<IOrganizationService> _lazyOrganizationService;
        private readonly Lazy<IAccidentService> _lazyAccidentService;
        private readonly Lazy<IRoleService> _lazyRoleService;
        private readonly Lazy<ISmartBraceletService> _lazySmartBraceletService;
        private readonly Lazy<IWorkShiftService> _lazyWorkShiftService;
        private readonly Lazy<IIssueService> _lazyIssueService;
        
        public ServiceManager(IRepositoryManager repository)
        {
            _lazyNewUserService = new Lazy<INewUserService>(() => new NewUserService(repository));
            _lazyOrganizationService = new Lazy<IOrganizationService>(() => new OrganizationService(repository));
            _lazyAccidentService = new Lazy<IAccidentService>(() => new AccidentService(repository));
            _lazyRoleService = new Lazy<IRoleService>(() => new RoleService(repository));
            _lazySmartBraceletService = new Lazy<ISmartBraceletService>(() => new SmartBraceletService(repository));
            _lazyWorkShiftService = new Lazy<IWorkShiftService>(() => new WorkShiftService(repository));
            _lazyIssueService = new Lazy<IIssueService>(() => new IssueService(repository));
        }
        public INewUserService UserService => _lazyNewUserService.Value;
        public IOrganizationService OrganizationService => _lazyOrganizationService.Value;
        public IAccidentService AccidentService => _lazyAccidentService.Value;
        public IRoleService RoleService => _lazyRoleService.Value;
        public ISmartBraceletService SmartBraceletService => _lazySmartBraceletService.Value;
        public IWorkShiftService WorkShiftService => _lazyWorkShiftService.Value;
        public IIssueService IssueService => _lazyIssueService.Value;
    }
}
