using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.Response;
using Service.Helpers;
using Service.Helpers.Services.ChannelsStrategies;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ChannelsTests
{
    public class SmtpSenderTests
    {
        public SmtpSenderTests(){}

        [Fact]
        public async Task SendAsync_WhenReciveValidMessage_ReturnSuccessResult()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var smtpConfiguration = channelConfig;
            var message = new SmtpMessage
            {
                Body = "Test",
                Subject = "Test"
            };
            var service = new SmtpChannelStrategy(smtpConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SendAsync_WhenReciveInvalidMessage_RetunFailedResult()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var smtpConfiguration = channelConfig;
            smtpConfiguration.SmtpConfiguration.To = "dadadadsa";
            var message = new SmtpMessage
            {
                Subject = "Test",
                Body = "Test"
            };
            var service = new SmtpChannelStrategy(smtpConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.False(result.IsSuccess);
        }
    }
}
