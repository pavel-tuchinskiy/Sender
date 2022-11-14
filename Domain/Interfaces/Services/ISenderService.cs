using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;

namespace Domain.Interfaces.Services
{
    public interface ISenderService
    {
        Task<bool> SendRangeAsync<T>(List<T> projects, List<Effect> effects, Templates templates);
        Task TelegramSpamToUser(string phone, int messageCount, string message);
    }
}
