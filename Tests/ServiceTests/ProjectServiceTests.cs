using Domain.Models.Project;
using Domain.Models.Rules.RuleModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.Services;
using System.Collections.Generic;
using Xunit;

namespace Tests.ServiceTests
{
    public class ProjectServiceTests
    {
        public List<Project> projects;
        public List<Rule> rules;
        public ProjectServiceTests()
        {
            projects = new List<Project>()
            {
                new Project()
                {
                    Id = 1,
                    Name = "Project 1",
                    Description = "Project Description 1",
                    Stage = "planning",
                    Categories = new List<int> { 2, 5, 6 },
                    Created_At = (long)1549459567,
                    Modified_At = (long)1549459594
                },
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
            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("C:\\SenderProject\\rules.json");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x.GetSection(It.Is<string>(k => k == Service.Constants.RULES_PATH))).Returns(mockSection.Object);
            var ruleService = new RuleService(mockConfiguration.Object);
            rules = ruleService.GetRules();
        }

        [Fact]
        public void FilterProjects_WhenCalled_RerunsFilteredProjectList()
        {
            //Arrange
            var service = new ProjectService();

            //Act
            var result = service.FilterProjects(projects, rules[0]);

            //Assert
            Assert.Single(result);
            Assert.Equal(projects[1], result[0]);
        }
    }
}
