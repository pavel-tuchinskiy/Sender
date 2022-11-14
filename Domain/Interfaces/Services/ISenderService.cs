using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;
using LanguageExt.Common;

namespace Domain.Interfaces.Services
{
    public interface ISenderService
    {
        Task<Result<bool>> SendRangeAsync<T>(List<T> projects, List<Effect> effects, Templates templates);
        Task<Result<bool>> TelegramSpamToUser(string phone, int messageCount, string message);
    }
}
