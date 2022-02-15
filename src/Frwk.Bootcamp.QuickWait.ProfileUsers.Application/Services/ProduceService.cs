using Confluent.Kafka;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Constants;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces.Service;
using Newtonsoft.Json;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class ProduceService : IProduceService
    {
        private readonly ClientConfig cloudConfig;
        public ProduceService()
        {
            cloudConfig = new ClientConfig
            {
                BootstrapServers = Settings.Kafkahost
                //BootstrapServers = CloudKarafka.Brokers,
                //SaslUsername = CloudKarafka.Username,
                //SaslPassword = CloudKarafka.Password,
                //SaslMechanism = SaslMechanism.ScramSha256,
                //SecurityProtocol = SecurityProtocol.SaslSsl,
                //EnableSslCertificateVerification = false
            };
        }
        public async Task Call(MessageInput message, string topicName)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);

            using var producer = new ProducerBuilder<string, string>(cloudConfig).Build();

            var key = new Guid().ToString();

            //var topic = string.Concat(CloudKarafka.Prefix, topicName);

            await producer.ProduceAsync(topicName, new Message<string, string> { Key = key, Value = stringfiedMessage });

            producer.Flush(TimeSpan.FromSeconds(2));
        }
    }
}
