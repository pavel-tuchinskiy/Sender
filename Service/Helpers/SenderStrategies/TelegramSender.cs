using Domain.Interfaces.Helpers;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Serilog;
using TL;

namespace Service.Helpers.SenderStrategies
{
    public class TelegramSender : ISender
    {
        private readonly TelegramConfiguration _telegramConfig;

        public TelegramSender(TelegramConfiguration configuration)
        {
            _telegramConfig = configuration;
        }

        public async Task<bool> SendAsync(Domain.Models.Message.Message message)
        {
            var telegramMessage = (TelegramMessage)message;
            Log.Information("Sending telegram message: \n {message}", telegramMessage.Body);
            using (var client = new WTelegram.Client(ClientConnection))
            {
                var user = await client.LoginUserIfNeeded();
                var userToSend = await client.Contacts_ResolvePhone(_telegramConfig.Recepient_Phone);
                var res = await client.SendMessageAsync(userToSend, telegramMessage.Body);
                Log.Information("Telegram result: {res}", res);

                return res != null ? true : false;
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
