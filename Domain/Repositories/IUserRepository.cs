using Marc2.Domain.Entities;

namespace Marc2.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetPrioritizedUsersByOrganizationAsync(int organizationId, User perpetrator);
        Task<IEnumerable<User>> GetUsersByOrganizationAsync(int organizationId, int perpetratorId);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
