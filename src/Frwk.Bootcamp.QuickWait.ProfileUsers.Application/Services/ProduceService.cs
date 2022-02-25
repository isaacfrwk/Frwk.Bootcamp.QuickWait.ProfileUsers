using Confluent.Kafka;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class ProduceService : IProduceService
    {
        private readonly ClientConfig cloudConfig;
        private readonly IConfiguration _configuration;
        public ProduceService(IConfiguration configuration)
        {
            _configuration = configuration;

            cloudConfig = new ClientConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka")["Host"]
            };
        }
        public async Task Call(MessageInput message, string topicName)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}
