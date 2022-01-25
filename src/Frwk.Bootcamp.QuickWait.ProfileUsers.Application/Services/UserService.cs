using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(User entity)
        {
            await _userRepository.AddAsync(entity);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            _userRepository.DeleteSync(entity);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteManyAsync(IEnumerable<User> entities)
        {
            _userRepository.DeleteManySync(entities);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            return await _userRepository.FindAllAsync(asNoTracking);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            return await _userRepository.FindAsync(predicate, asNoTracking);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            await _userRepository.UpdateAsync(entity);
            await _userRepository.SaveChangesAsync();
        }
    }
}
