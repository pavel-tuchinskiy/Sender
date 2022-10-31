using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;

namespace Domain.Interfaces.Services
{
    public interface IChannel
    {
        Task<bool> SendRangeAsync<T>(List<T> objects, Effect effect, Templates templates);
    }
}
