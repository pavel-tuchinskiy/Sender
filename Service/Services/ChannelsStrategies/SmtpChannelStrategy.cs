using Domain.Interfaces.Strategy;
using Domain.Models.Configuration;
using Domain.Models.Message;
using Domain.Models.Response;
using Domain.Models.Rules.EffectModels;
using LanguageExt.Common;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using Serilog;

namespace Service.Helpers.Services.ChannelsStrategies
{
    public class SmtpChannelStrategy : IChannel
    {
        private readonly SmtpConfiguration _smtpConfig;
        public readonly ChannelType _channelType = ChannelType.SMTP;

        public SmtpChannelStrategy(ChannelsConfiguration configuration)
        {
            _smtpConfig = configuration.SmtpConfiguration;
        }

        public async Task<Result<bool>> SendAsync(Message message)
        {
            var smtpMessage = (SmtpMessage)message;
            Log.Debug("Sending email message: \n {subject} \n {body}", smtpMessage.Subject, smtpMessage.Body);
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_smtpConfig.From.Name, _smtpConfig.From.Email));
            email.To.Add(new MailboxAddress("", _smtpConfig.To));
            email.Subject = smtpMessage.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = smtpMessage.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfig.SmtpServer, _smtpConfig.Port, true);
                await client.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);

                try
                {
                    var res = await client.SendAsync(email);
                }
                catch (MailKit.Net.Smtp.SmtpCommandException)
                {
                    Log.Error("Can't send message: {message}", JsonConvert.SerializeObject(message));
                    var ex = new ResponseException("Error while trying to send a message");
                    return new Result<bool>(ex);
                }

                await client.DisconnectAsync(true);

                return true;
            }
        }
    }
}
