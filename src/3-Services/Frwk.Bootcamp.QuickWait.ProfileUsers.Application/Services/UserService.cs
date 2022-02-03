using Confluent.Kafka;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Constants;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Contracts;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly string topicName;
        private readonly ClientConfig cloudConfig;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.topicName = Settings.topicName;

            cloudConfig = new ClientConfig
            {
                BootstrapServers = Settings.Kafka
            };
        }

        public async Task AddAsync(User entity)
        {
            await userRepository.AddAsync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(Settings.User, MethodConstant.POST, JsonConvert.SerializeObject(entity));

            await this.Call(message);
        }

        public async Task DeleteAsync(User entity)
        {
            userRepository.DeleteSync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(Settings.User, MethodConstant.DELETE, JsonConvert.SerializeObject(entity));

            await this.Call(message);
        }

        public async Task DeleteManyAsync(IEnumerable<User> entities)
        {
            userRepository.DeleteManySync(entities);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(Settings.User, MethodConstant.DELETEMANY, JsonConvert.SerializeObject(entities));

            await this.Call(message);

        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            return await userRepository.FindAllAsync(asNoTracking);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            return await userRepository.FindAsync(predicate, asNoTracking);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            await userRepository.UpdateAsync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(Settings.User, MethodConstant.PUT, JsonConvert.SerializeObject(entity));

            await this.Call(message);
        }


        protected async Task Call(MessageInput message)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(2));
        }
    }
}
