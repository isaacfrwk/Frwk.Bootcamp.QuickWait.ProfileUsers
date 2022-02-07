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
        private readonly string topicNameUser;
        private readonly string topicNameUserResponse;
        private readonly ClientConfig cloudConfig;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.topicNameUser = Topics.topicNameUser;
            this.topicNameUserResponse = Topics.topicNameUserResponse;

            cloudConfig = new ClientConfig
            {
                BootstrapServers = CloudKarafka.Brokers,
                SaslUsername = CloudKarafka.Username,
                SaslPassword = CloudKarafka.Password,
                SaslMechanism = SaslMechanism.ScramSha256,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                EnableSslCertificateVerification = false
            };
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
                throw new Exception(ex.Message, ex);
            }
            
            var message = new MessageInput(null, MethodConstant.POST, JsonConvert.SerializeObject(entity));

            await this.Call(message, topicNameUser);
        }

        public async Task DeleteAsync(User entity)
        {
            userRepository.DeleteSync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.DELETE, JsonConvert.SerializeObject(entity));

            await this.Call(message, topicNameUser);
        }

        public async Task DeleteManyAsync(IEnumerable<User> entities)
        {
            userRepository.DeleteManySync(entities);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.DELETEMANY, JsonConvert.SerializeObject(entities));

            await this.Call(message, topicNameUser);

        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            var users = await userRepository.FindAllAsync(asNoTracking);
            MessageInput? message = null;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await this.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            var users = await userRepository.FindAsync(predicate, asNoTracking);

            MessageInput? message = null;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await this.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var users = await userRepository.GetByIdAsync(id);

            MessageInput? message = null;

            if (users != null)
                message = new MessageInput(200, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));
            message = new MessageInput(404, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await this.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task UpdateAsync(User entity)
        {
            await userRepository.UpdateAsync(entity);
            await userRepository.SaveChangesAsync();

            var message = new MessageInput(null, MethodConstant.PUT, JsonConvert.SerializeObject(entity));

            await this.Call(message, topicNameUser);
        }


        protected async Task Call(MessageInput message, string topicName)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            await producer.ProduceAsync($"{CloudKarafka.Prefix + topicName}", new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(2));
        }
    }
}
