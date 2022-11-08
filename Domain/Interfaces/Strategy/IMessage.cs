using Domain.Models.Message;

namespace Domain.Interfaces.Strategy
{
    public interface IMessage
    {
        List<Message> CreateMessages<T>(List<T> items);
    }
}
