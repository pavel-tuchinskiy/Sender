using Domain.Models.Project;
using Domain.Models.Response;
using Domain.Models.Rules;
using Domain.Models.Rules.RuleModels;
using Service.Helpers;
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
            var jParser = new JsonParser();
            rules = jParser.DeserializeFile<RulesRoot>("C:\\SenderProject\\rules.json").Rules;
        }

        [Fact]
        public void FilterProjects_WhenCalled_RerunsSuccessResult()
        {
            //Arrange
            var service = new ProjectService();

            //Act
            var result = service.FilterProjects(projects, rules[0]);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void FilterProjects_IfProjectsIsNull_ReturnFailedResult()
        {
            //Arrange
            var service = new ProjectService();
            List<Project> projects = null;

            //Act
            var result = service.FilterProjects(projects, rules[0]);

            //Assert
            Assert.False(result.IsSuccess);
        }
    }
}
