using Domain.Models.Configuration;
using Domain.Models.Message;
using Service.Helpers;
using Service.Helpers.SenderStrategies;
using System.Threading.Tasks;
using Xunit;

namespace Tests.HelpersTests
{
    public class SmtpSenderTests
    {
        public SmtpSenderTests(){}

        [Fact]
        public async Task SendAsync_WhenCalled_SendEmail()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var smtpConfiguration = channelConfig.SmtpConfiguration;
            var message = new SmtpMessage
            {
                Body = "Test",
                Subject = "Test"
            };
            var service = new SmtpSender(smtpConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.True(result);
        }
    }
}
