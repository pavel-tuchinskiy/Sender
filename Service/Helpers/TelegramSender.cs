using Domain.Models.Configuration;
using Domain.Models.Message;
using TL;

namespace Service.Helpers
{
    public class TelegramSender
    {
        private readonly TelegramConfiguration _telegramConfig;

        public TelegramSender(TelegramConfiguration configuration)
        {
            _telegramConfig = configuration;
        }

        public async Task SendAsync(TelegramMessage telegramMessage)
        {
            using (var client = new WTelegram.Client(ClientConnection))
            {
                var user = await client.LoginUserIfNeeded();
                var userToSend = await client.Contacts_ResolvePhone(_telegramConfig.Recepient_Phone);
                await client.SendMessageAsync(userToSend, telegramMessage.Body);
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
