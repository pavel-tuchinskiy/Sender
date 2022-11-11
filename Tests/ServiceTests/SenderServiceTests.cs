using Domain.Models.Project;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ServiceTests
{
    public class SenderServiceTests
    {
        public List<Project> projects;
        public List<Rule> rules;
        public SenderServiceTests()
        {
            projects = new List<Project>()
            {
                 new Project()
                 {
                    Id = 2,
                    Name = "Project 2",
                    Description = "Project Description 2",
                    Stage = "planning",
                    Categories = new List<int> { 2, 6 },
                    Created_At = (long)1549459243,
                    Modified_At = (long)1549459564
                 }
            };
            Mock<IConfigurationSection> mockRules = new Mock<IConfigurationSection>();
            mockRules.Setup(x => x.Value).Returns("C:\\SenderProject\\rules.json");
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.RULES_PATH))).Returns(mockRules.Object);
            var ruleService = new RuleService(mockConfig.Object);
            rules = ruleService.GetRules();
        }

        [Fact]
        public async Task SendRangeAsync_WhenCalled_SendObjectListViaChannels()
        {
            //Arrange
            Mock<IConfigurationSection> mockChannels = new Mock<IConfigurationSection>();
            mockChannels.Setup(x => x.Value).Returns("C:\\SenderProject\\senderconfig.json");

            Mock<IConfigurationSection> mockTemplates = new Mock<IConfigurationSection>();
            mockTemplates.Setup(x => x.Value).Returns("C:\\SenderProject\\templates.json");

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.CHANNELS_CONFIG))).Returns(mockChannels.Object);
            mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.TEMPLATES_PATH))).Returns(mockTemplates.Object);
            var service = new SenderService(mockConfiguration.Object);

            //Act
            var result = await service.SendRangeAsync(projects, rules[0].Effects);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendRangeAsync_WhenEffectIsNull_ThrowException()
        {
            //Arrange
            Mock<IConfigurationSection> mockChannels = new Mock<IConfigurationSection>();
            mockChannels.Setup(x => x.Value).Returns("C:\\SenderProject\\senderconfig.json");

            Mock<IConfigurationSection> mockTemplates = new Mock<IConfigurationSection>();
            mockTemplates.Setup(x => x.Value).Returns("C:\\SenderProject\\templates.json");

            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.CHANNELS_CONFIG))).Returns(mockChannels.Object);
            mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.TEMPLATES_PATH))).Returns(mockTemplates.Object);
            var service = new SenderService(mockConfiguration.Object);
            List<Effect> effects = null;

            //Act
            var ex = await Assert.ThrowsAsync<ResponseException>(() => service.SendRangeAsync(projects, effects));

            //Assert
            Assert.IsType<ResponseException>(ex);
        }
    }
}
