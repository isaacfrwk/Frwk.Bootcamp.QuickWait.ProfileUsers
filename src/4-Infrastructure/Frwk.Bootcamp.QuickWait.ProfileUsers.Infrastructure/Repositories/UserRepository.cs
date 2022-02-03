using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.UserContext
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dBContext;
        public UserRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task AddAsync(User entity)
        {
            await _dBContext.Users.AddAsync(entity);
        }

        public void DeleteSync(User entity)
        {
            _dBContext.Users.Remove(entity);
        }

        public void DeleteManySync(IEnumerable<User> entities)
        {
            _dBContext.Users.RemoveRange(entities);
        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return await _dBContext.Users
                    .Include(x => x.Address).AsNoTracking().ToListAsync();
            }

            return await _dBContext.Users
                .Include(x => x.Address).ToListAsync();
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return await _dBContext.Users
                    .Include(x => x.Address).Where(predicate).AsNoTracking().ToListAsync();
            }

            return await _dBContext.Users
                .Include(x => x.Address).Where(predicate).ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var item = await _dBContext.Users
                .Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);

            if (item is null)
            {
                return null;
            }

            return item;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            var item = await _dBContext.Users
                .Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (item != null)
            {
                _dBContext.Entry(item).CurrentValues.SetValues(entity);
                item.Address = entity.Address;
            }
        }
    }
}
