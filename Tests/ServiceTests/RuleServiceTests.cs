using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.Services;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace Tests.ServiceTests
{
    public class RuleServiceTests
    {
        public List<Rule> rules = new List<Rule>()
        {
            new Rule()
            {
                Operator = Operator.And,
                Conditions = new List<RuleCondition> { 
                    new RuleCondition()
                    {
                        Key = "categories",
                        Condition = Conditions.InArray,
                        Value = (long)2
                    }
                },
                Effects = new List<Effect>
                {
                    new Effect(){
                        Type = ChannelType.Telegram,
                        TemplateId = 1
                    }
                }
            }
        };

        public Mock<IConfiguration> _mockConfiguration;
        public RuleServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            var placeholder1 = new ExpandoObject();
            placeholder1.TryAdd("id", "id");
            placeholder1.TryAdd("name", "name");
            rules[0].Effects[0].Placeholders = placeholder1;
        }

        [Fact]
        public void GetRules_WhenCalled_ReturnsRuleList()
        {
            //Arrange
            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("C:\\SenderProject\\rules.json");
            _mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.RULES_PATH))).Returns(mockSection.Object);
            var service = new RuleService(_mockConfiguration.Object);

            //Act
            var result = service.GetRules();

            //Assert
            Assert.Equal(3, result[0].Conditions.Count);
            Assert.Equal(2, result[0].Effects.Count);
            Assert.Equal(rules[0].Operator, result[0].Operator);
            Assert.Equivalent(rules[0].Conditions[0], result[0].Conditions[0]);
            Assert.Equivalent(rules[0].Effects[0], result[0].Effects[0]);
        }

        [Fact]
        public void RuleService_WhenFileNotExist_ThrowsException()
        {
            //Arrange
            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("C:\\SenderProject\\notexistjson.json");
            _mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.RULES_PATH))).Returns(mockSection.Object);
            var service = new RuleService(_mockConfiguration.Object);

            //Act
            var ex = Assert.Throws<ResponseException>(() => service.GetRules());

            //Assert
            Assert.IsType<ResponseException>(ex);
        }
    }
}