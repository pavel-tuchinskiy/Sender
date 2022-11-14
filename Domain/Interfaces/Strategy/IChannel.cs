using Domain.Models.Message;
using LanguageExt.Common;

namespace Domain.Interfaces.Strategy
{
    public interface IChannel
    {
        Task<Result<bool>> SendAsync(Message message);
    }
}
