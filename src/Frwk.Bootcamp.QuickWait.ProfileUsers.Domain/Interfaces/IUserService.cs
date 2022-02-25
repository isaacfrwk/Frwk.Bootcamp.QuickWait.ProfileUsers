using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces
{
    public interface IUserService
    {
        Task InsertAsync(User entity);

        Task DeleteAsync(User entity);

        Task DeleteManyAsync(IEnumerable<User> entities);

        Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true);

        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true);

        Task<User> GetByIdAsync(Guid id);

        Task UpdateAsync(User entity);
    }
}
