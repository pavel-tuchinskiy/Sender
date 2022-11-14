using Domain.Models.Configuration;
using Domain.Models.MessageTemplates;
using Domain.Models.Project;
using Domain.Models.Response;
using Domain.Models.Rules;
using Domain.Models.Rules.EffectModels;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Options;
using Moq;
using Service.Helpers;
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
        public Mock<IOptionsSnapshot<ChannelsConfiguration>> configMock;
        public Templates templates;
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
            var jParser = new JsonParser();
            rules = jParser.DeserializeFile<RulesRoot>("C:\\SenderProject\\rules.json").Rules;
            var configuration = jParser.DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            configMock = new Mock<IOptionsSnapshot<ChannelsConfiguration>>();
            configMock.SetupGet(x => x.Value).Returns(configuration);
            templates = jParser.DeserializeFile<Templates>("C:\\SenderProject\\templates.json");
        }

        [Fact]
        public async Task SendRangeAsync_WhenCalled_SendObjectListViaChannels()
        {
            //Arrange
            var service = new SenderService(configMock.Object);

            //Act
            var result = await service.SendRangeAsync(projects, rules[0].Effects, templates);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendRangeAsync_WhenEffectIsNull_ThrowException()
        {
            //Arrange
            var service = new SenderService(configMock.Object);
            List<Effect> effects = null;

            //Act
            var ex = await Assert.ThrowsAsync<ResponseException>(() => service.SendRangeAsync(projects, effects, templates));

            //Assert
            Assert.IsType<ResponseException>(ex);
        }
    }
}
