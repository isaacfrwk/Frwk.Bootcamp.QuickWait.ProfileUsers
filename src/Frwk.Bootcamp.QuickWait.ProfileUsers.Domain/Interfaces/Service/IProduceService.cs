using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces.Service
{
    public interface IProduceService
    {
        Task Call(MessageInput message, string topicName);
    }
}
