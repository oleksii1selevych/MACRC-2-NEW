using Marc2.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Marc2.Persistence.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        public RepositoryBase(ApplicationDbContext context)
        {
            ApplicationDbContext = context;
        }
        public void Create(T entity) => ApplicationDbContext.Set<T>().Add(entity);

        public void Delete(T entity) => ApplicationDbContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll() => ApplicationDbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
            => ApplicationDbContext.Set<T>().Where(expression);

        public void Update(T entity) => ApplicationDbContext.Set<T>().Update(entity);
    }
}
