using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.Response;
using Service.Helpers;
using Service.Helpers.Services.ChannelsStrategies;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ChannelsTests
{
    public class TelegramSenderTests
    {
        public TelegramSenderTests(){}

        [Fact]
        public async Task SendAsync_WhenReciveValidMessage_ReturnSuccessResult()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var telegramConfiguration = channelConfig;
            var message = new TelegramMessage
            {
                Body = "Test"
            };
            var service = new TelegramChannelStrategy(telegramConfiguration);

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
            var telegramConfiguration = channelConfig;
            TelegramMessage message = new TelegramMessage();
            var service = new TelegramChannelStrategy(telegramConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.False(result.IsSuccess);
        }
    }
}
