using Core.Helpers;
using Domain.Attributes;
using Domain.Interfaces.Strategy;
using Domain.Models.MessageTemplates;
using Domain.Models.Rules.EffectModels;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Service.Helpers.Services.MessagesStrategies
{
    public class MessageStrategyResolver
    {
        public IMessage GetMessageStrategy(Effect effect, Templates templates)
        {
            var placeholders = effect.Placeholders.Select(x => x.Key).ToList();

            var template = GetTemplate(templates, effect.Type, effect.Template_id);
            var messageStrategies = StrategiesHelper.GetStrategies<ChannelType, IMessage>(typeof(MessageStrategyResolver), template, placeholders);

            return messageStrategies[effect.Type];
        }

        private TemplateBase GetTemplate(Templates templates, ChannelType channelType, int templateId)
        {
            var props = typeof(Templates).GetProperties();
            foreach(var prop in props)
            {
                var type = prop.GetCustomAttribute<ChannelTypeAttribute>()!.ChannelType;
                
                if(type == channelType)
                {
                    var templatesList = (IEnumerable<TemplateBase>)prop.GetValue(templates);

                    var template = templatesList.FirstOrDefault(x => x.Id == templateId);
                    return template;
                }
            }
            return null;
        }
    }
}
