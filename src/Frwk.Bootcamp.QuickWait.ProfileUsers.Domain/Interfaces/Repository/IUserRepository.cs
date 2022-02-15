using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts
{
    public interface IUserRepository
    {
        Task AddAsync(User entity);
        void DeleteSync(User entity);
        void DeleteManySync(IEnumerable<User> entities);
        Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true);
        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true);
        Task<User> GetByIdAsync(Guid id);
        Task<int> SaveChangesAsync();
        Task UpdateAsync(User entity);
        Task<User> SaveLast();
    }
}
