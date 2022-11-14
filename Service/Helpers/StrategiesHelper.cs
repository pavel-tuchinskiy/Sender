using Domain.Models.Rules.EffectModels;
using System.Reflection;

namespace Core.Helpers
{
    public static class StrategiesHelper
    {
        public static Dictionary<TKey, TValue> GetStrategies<TKey, TValue>(Type assambly, params object[] parameters)
        {
            var senderTypes = Assembly.GetAssembly(assambly)
               .GetTypes()
               .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(TValue)));

            var sendersDic = new Dictionary<TKey, TValue>();

            foreach (var senderType in senderTypes)
            {
                var instance = Activator.CreateInstance(senderType, parameters);
                var field = senderType.GetFields().FirstOrDefault(x => x.FieldType == typeof(TKey));
                var senderKey = (TKey)field.GetValue(instance);
                sendersDic.Add(senderKey, (TValue)instance);
            }

            return sendersDic;
        }
    }
}
