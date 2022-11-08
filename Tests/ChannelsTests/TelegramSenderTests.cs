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
        public async Task SendAsync_WhenReciveValidMessage_SendTelegramMessage()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var telegramConfiguration = channelConfig.TelegramConfiguration;
            var message = new TelegramMessage
            {
                Body = "Test"
            };
            var service = new TelegramChannelStrategy(telegramConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendAsync_WhenReciveInvalidMessage_ThrowsException()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var telegramConfiguration = channelConfig.TelegramConfiguration;
            TelegramMessage message = new TelegramMessage();
            var service = new TelegramChannelStrategy(telegramConfiguration);

            //Act
            var ex = await Assert.ThrowsAsync<ResponseException>(() => service.SendAsync(message));

            //Assert
            Assert.IsType<ResponseException>(ex);
        }
    }
}
