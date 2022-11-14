using Domain.Models.Rules.RuleModels;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Service.Extensions
{
    public static class ProjectQueriableExtensions
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

                queryBuilder.Append(HandleRuleCondition(prop, condition));
                queryBuilder.Append(op);
            }

            var query = queryBuilder.ToString().TrimEnd(new char[] {' ', '&', '|'});

            return source.Where(query);
        }

        private static string HandleRuleCondition(string property, RuleCondition ruleCondition)
        {
            return ruleCondition.Condition switch
            {
                Conditions.InArray => $"{property}.Contains({ruleCondition.Val})",
                Conditions.Equal => $"{property}.ToString().Equals(\"{ruleCondition.Val}\")",
                Conditions.MoreThan => $"{property} > {ruleCondition.Val}",
                Conditions.LessThan => $"{property} < {ruleCondition.Val}",
                _ => string.Empty
            };
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
