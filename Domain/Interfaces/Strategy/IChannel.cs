using Domain.Models.Message;

namespace Domain.Interfaces.Strategy
{
    public interface IChannel
    {
        Task<bool> SendAsync(Message message);
    }
}
