using Core;
using Domain.Interfaces.Options;
using Domain.Models.Configuration;
using System.Reflection;

namespace Web.Configuration
{
    public static class WebConfiguration
    {
        public static void AddSenderOptions(this WebApplicationBuilder builder)
        {
            AddConfigurationFiles(builder.Configuration, typeof(ConfigKeysStorage));

            RegisterSenderOptions(builder);
        }

        private static void AddConfigurationFiles(ConfigurationManager configrationManager, Type keysStorage)
        {
            var fields = keysStorage.GetFields();
            foreach (var field in fields)
            {
                var key = (string)field.GetValue(null);
                var path = configrationManager.GetSection(key).Value;
                configrationManager.AddJsonFile(path);
            }
        }

        private static void RegisterSenderOptions(WebApplicationBuilder builder)
        {
            var options = GetOptionsTypes<ISenderOptions>();

            foreach(var option in options)
            {
                RegisterOption(builder.Services, builder.Configuration, option);
            }
        }

        private static List<Type> GetOptionsTypes<TOptionType>()
        {
            var options = Assembly.GetAssembly(typeof(ChannelsConfiguration))
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(TOptionType)))
                .ToList();

            return options;
        }

        public static void RegisterOption(IServiceCollection services, IConfiguration configuration, Type option)
        {
            var myMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
              .GetMethods(BindingFlags.Static | BindingFlags.Public)
              .Where(x => x.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure) && x.IsGenericMethodDefinition)
              .Where(x => x.GetGenericArguments().Length == 1)
              .Where(x => x.GetParameters().Length == 2)
              .First();

            MethodInfo generic = myMethod.MakeGenericMethod(option);
            generic.Invoke(null, new object[] { services, configuration });
        }
    }
}
