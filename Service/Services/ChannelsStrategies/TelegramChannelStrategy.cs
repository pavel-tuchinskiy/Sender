using Domain.Interfaces.Strategy;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using Newtonsoft.Json;
using Serilog;
using TL;
using Message = Domain.Models.Message.Message;

namespace Service.Helpers.Services.ChannelsStrategies
{
    public class TelegramChannelStrategy : IChannel
    {
        private readonly TelegramConfiguration _telegramConfig;
        public readonly ChannelType _channelType = ChannelType.Telegram;

        public TelegramChannelStrategy(ChannelsConfiguration configuration)
        {
            _telegramConfig = configuration.TelegramConfiguration;
        }

        public async Task<bool> SendAsync(Message message)
        {
            var telegramMessage = (TelegramMessage)message;
            Log.Debug("Sending telegram message: \n {message}", telegramMessage.Body);
            using (var client = new WTelegram.Client(ClientConnection))
            {
                var user = await client.LoginUserIfNeeded();
                var userToSend = await client.Contacts_ResolvePhone(_telegramConfig.Recepient_Phone);
                try
                {
                    var res = await client.SendMessageAsync(userToSend, telegramMessage.Body);
                }
                catch (TL.RpcException)
                {
                    Log.Error("Can't send message: {message}", JsonConvert.SerializeObject(message));
                    throw new ResponseException("Error while trying to send a message");
                }

                return true;
            }
        }

        public async Task SendManyAsync(TelegramMessage telegramMessage, int messageCount)
        {
            using (var client = new WTelegram.Client(ClientConnection))
            {
                var user = await client.LoginUserIfNeeded();
                var userToSend = await client.Contacts_ResolvePhone(_telegramConfig.Recepient_Phone);

                var random = new Random();
                for (int i = 0; i < messageCount; i++)
                {
                    var msec = random.Next(3, 10) * 1000;
                    await client.SendMessageAsync(userToSend, telegramMessage.Body);
                    Task.Delay(msec).Wait();
                }
            }
        }

        private string ClientConnection(string required)
        {
            switch (required)
            {
                case "api_id": return _telegramConfig.Api_Id;
                case "api_hash": return _telegramConfig.Api_Hash;
                case "phone_number": return _telegramConfig.Phone;
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                default: return null;
            }
        }
    }
}
