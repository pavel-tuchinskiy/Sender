using Domain.Models.Configuration;
using Domain.Models.Message;
using Service.Helpers;
using Service.Helpers.SenderStrategies;
using System.Threading.Tasks;
using Xunit;

namespace Tests.HelpersTests
{
    public class TelegramSenderTests
    {
        public TelegramSenderTests(){}

        [Fact]
        public async Task SendAsync_WhenCalled_SendTelegramMessage()
        {
            //Arrange
            var channelConfig = new JsonParser().DeserializeFile<ChannelsConfiguration>("C:\\SenderProject\\senderconfig.json");
            var telegramConfiguration = channelConfig.TelegramConfiguration;
            var message = new TelegramMessage
            {
                Body = "Test"
            };
            var service = new TelegramSender(telegramConfiguration);

            //Act
            var result = await service.SendAsync(message);

            //Assert
            Assert.True(result);
        }
    }
}
