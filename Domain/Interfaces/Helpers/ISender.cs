using Domain.Models.Message;

namespace Domain.Interfaces.Helpers
{
    public interface ISender
    {
        Task<bool> SendAsync(Message message);
    }
}
