using Domain.Models;
using Domain.Models.Rules.EffectModels;

namespace Domain.Interfaces.Services
{
    public interface ISenderService
    {
        Task SendRangeAsync<T>(List<T> projects, List<Effect> effects);
        Task TelegramSpamToUser(string phone, int messageCount, string message);
    }
}
