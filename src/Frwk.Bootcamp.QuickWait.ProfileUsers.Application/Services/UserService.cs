using Confluent.Kafka;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Constants;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces.Service;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly string topicNameUser;
        private readonly string topicNameUserResponse;
        private readonly IProduceService produceService;
        public UserService(IUserRepository userRepository, IProduceService produceService)
        {
            this.userRepository = userRepository;
            this.topicNameUser = Topics.topicNameUser;
            this.topicNameUserResponse = Topics.topicNameUserResponse;
            this.produceService = produceService;

        }

        public async Task AddAsync(User entity)
        {
            try
            {
                await userRepository.AddAsync(entity);
                await userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await produceService.Call(new MessageInput(400, MethodConstant.POST, "erro ao tentar salvar o usuário"), topicNameUserResponse);
            }
            
            var message = new MessageInput(null, MethodConstant.POST, JsonConvert.SerializeObject(entity));

            await produceService.Call(message, topicNameUser);

        }

        public async Task DeleteAsync(User entity)
        {
            userRepository.DeleteSync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.DELETE, JsonConvert.SerializeObject(entity));

            await produceService.Call(message, topicNameUser);
        }

        public async Task DeleteManyAsync(IEnumerable<User> entities)
        {
            userRepository.DeleteManySync(entities);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.DELETEMANY, JsonConvert.SerializeObject(entities));

            await produceService.Call(message, topicNameUser);

        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            var users = await userRepository.FindAllAsync(asNoTracking);
            MessageInput? message;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            else
                message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            var users = await userRepository.FindAsync(predicate, asNoTracking);

            MessageInput? message;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            else
                message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var users = await userRepository.GetByIdAsync(id);

            MessageInput? message;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            else
                message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task UpdateAsync(User entity)
        {
            await userRepository.UpdateAsync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.PUT, JsonConvert.SerializeObject(entity));

            await produceService.Call(message, topicNameUser);
        }

    }
}
