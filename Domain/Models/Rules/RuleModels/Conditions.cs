using System.Runtime.Serialization;

namespace Domain.Models.Rules.RuleModels
{
    public enum Conditions
    {
        [EnumMember(Value = "equal")]
        Equal,
        [EnumMember(Value = "inArray")]
        InArray,
        [EnumMember(Value = "moreThan")]
        MoreThan,
        [EnumMember(Value = "lessThan")]
        LessThan
    }
}
