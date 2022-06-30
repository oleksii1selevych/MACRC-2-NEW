using Marc2.Domain.Repositories;

namespace Marc2.Persistence.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly Lazy<IUserRepository> _lazyUserRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;
        private readonly Lazy<IRoleRepository> _lazyRoleRepository;
        private readonly Lazy<IOrganizationRepository> _lazyOrganizationRepository;
        private readonly Lazy<IAccidentRepository> _lazyAccidentRepository;
        private readonly Lazy<ISmartBraceletRepository> _lazySmartBraceletRepository;
        private readonly Lazy<IWorkShiftRepository> _lazyWorkShiftRepository;
        private readonly Lazy<IIssueRepository> _lazyIssueRepository;
        private readonly Lazy<IAssignmentRepository> _lazyAssignmentRepository;

        public RepositoryManager(ApplicationDbContext dbContext)
        {
            _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
            _lazyRoleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(dbContext));
            _lazyOrganizationRepository = new Lazy<IOrganizationRepository>(() => new OrganizationRepository(dbContext));
            _lazyAccidentRepository = new Lazy<IAccidentRepository>(() => new AccidentRepository(dbContext));
            _lazySmartBraceletRepository = new Lazy<ISmartBraceletRepository>(() => new SmartBraceletRepository(dbContext));
            _lazyWorkShiftRepository = new Lazy<IWorkShiftRepository>(() => new WorkShiftRepository(dbContext));
            _lazyIssueRepository = new Lazy<IIssueRepository>(() => new IssueRepository(dbContext));
            _lazyAssignmentRepository = new Lazy<IAssignmentRepository>(() => new AssignmentRepository(dbContext));
        }

        public IUserRepository UserRepository => _lazyUserRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
        public IRoleRepository RoleRepository => _lazyRoleRepository.Value;
        public IOrganizationRepository OrganizationRepository => _lazyOrganizationRepository.Value;
        public IAccidentRepository AccidentRepository => _lazyAccidentRepository.Value;
        public ISmartBraceletRepository SmartBraceletRepository => _lazySmartBraceletRepository.Value;
        public IWorkShiftRepository WorkShiftRepository => _lazyWorkShiftRepository.Value;
        public IIssueRepository IssueRepository => _lazyIssueRepository.Value;
        public IAssignmentRepository AssignmentRepository => _lazyAssignmentRepository.Value;
    }
}
