using Marc2.Domain.Repositories;

namespace Marc2.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync() =>
             await _dbContext.SaveChangesAsync();

    }
}
