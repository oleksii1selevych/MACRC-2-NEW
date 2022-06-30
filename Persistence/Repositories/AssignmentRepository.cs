using Marc2.Domain.Entities;
using Marc2.Domain.Repositories;

namespace Marc2.Persistence.Repositories
{
    public class AssignmentRepository : RepositoryBase<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void DeleteAssignment(Assignment assignment)
            => Delete(assignment);
    }
}
