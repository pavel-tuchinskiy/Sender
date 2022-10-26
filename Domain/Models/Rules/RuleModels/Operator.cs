using System.Runtime.Serialization;

namespace Domain.Models.Rules.RuleModels
{
    public enum Operator
    {
        [EnumMember(Value = "and")]
        And,
        [EnumMember(Value = "or")]
        Or
    }
}
