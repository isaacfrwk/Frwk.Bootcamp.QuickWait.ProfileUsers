using Confluent.Kafka;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Constants;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Consumers
{
    public class UserConsumer : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly ConsumerConfig consumerConfig;
        public UserConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.topicName = Topics.topicName;

            this.consumerConfig = new ConsumerConfig
            {
                //BootstrapServers = CloudKarafka.Brokers,
                //SaslUsername = CloudKarafka.Username,
                //SaslPassword = CloudKarafka.Password,
                //SaslMechanism = SaslMechanism.ScramSha256,
                //SecurityProtocol = SecurityProtocol.SaslSsl,
                //EnableSslCertificateVerification = false,
                BootstrapServers = Settings.Kafkahost,
                GroupId = $"{topicName}-group-3",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = Task.Run(() => ProcessQueue(stoppingToken), stoppingToken);

            return task;
        }

        private void ProcessQueue(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(topicName);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        Task.Run(async () => { await InvokeService(consumeResult); }, stoppingToken);
                    }
                    catch (ConsumeException ex)
                    { }
                }
            }
            catch (OperationCanceledException ex)
            {
                consumer.Close();
            }
        }

        private async Task InvokeService(ConsumeResult<Ignore, string> message)
        {
            var mensagem = JsonConvert.DeserializeObject<MessageInput>(message.Message.Value);

            using var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetService<IUserService>();

            switch (mensagem.Method)
            {
                case MethodConstant.POST:
                    await service.AddAsync(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.PUT:
                    await service.UpdateAsync(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.DELETE:
                    await service.DeleteAsync(JsonConvert.DeserializeObject<User>(mensagem.Content));
                    break;
                case MethodConstant.DELETEMANY:
                    await service.DeleteManyAsync(JsonConvert.DeserializeObject<IEnumerable<User>>(mensagem.Content));
                    break;
                case MethodConstant.FINDALL:
                    await service.FindAllAsync();
                    break;
                case MethodConstant.GETBYID:
                    await service.GetByIdAsync(JsonConvert.DeserializeObject<Guid>(mensagem.Content));
                    break;

            }
        }
    }
}
