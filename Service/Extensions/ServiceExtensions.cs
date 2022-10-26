using Domain.Models.Rules.RuleModels;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Service.Extensions
{
    public static class ServiceExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, Rule rule)
        {
            if(rule == null)
            {
                return source;
            }

            var queryBuilder = new StringBuilder();

            var op = rule.Operator == Operator.And ? " && " : " || ";

            var props = GetProperties<T>();

            foreach (var condition in rule.Conditions)
            {
                if (condition == null)
                {
                    continue;
                }

                var prop = props[condition.Key];

                if (condition.Condition == Conditions.InArray)
                {
                    queryBuilder.Append($"{prop}.Contains({condition.Value})");
                }
                else if (condition.Condition == Conditions.Equal)
                {
                    queryBuilder.Append($"{prop}.ToString().Equals(\"{condition.Value}\")");
                }
                else if (condition.Condition == Conditions.MoreThan)
                {
                    queryBuilder.Append($"{prop} > {condition.Value}");
                }
                else if (condition.Condition == Conditions.LessThan)
                {
                    queryBuilder.Append($"{prop} < {condition.Value}");
                }

                queryBuilder.Append(op);
            }

            var query = queryBuilder.ToString().TrimEnd(new char[] {' ', '&', '|'});

            return source.Where(query);
        }

        private static Dictionary<string, string> GetProperties<T>()
        {
            var props = typeof(T).GetProperties();
            var propertiesDic = new Dictionary<string, string>();

            foreach(var prop in props)
            {
                var jsonProp = prop.GetCustomAttribute<JsonPropertyAttribute>()!.PropertyName;

                if(jsonProp == null)
                {
                    continue;
                }

                propertiesDic.Add(jsonProp, prop.Name);
            }

            return propertiesDic;
        }
    }
}
