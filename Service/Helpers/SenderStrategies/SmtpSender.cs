using Domain.Interfaces.Helpers;
using Domain.Models.Configuration;
using Domain.Models.Message;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace Service.Helpers.SenderStrategies
{
    public class SmtpSender : ISender
    {
        private readonly SmtpConfiguration _smtpConfig;

        public SmtpSender(SmtpConfiguration configuration)
        {
            _smtpConfig = configuration;
        }

        public async Task<bool> SendAsync(Message message)
        {
            var smtpMessage = (SmtpMessage)message;
            Log.Information("Sending email message: \n {subject} \n {body}", smtpMessage.Subject, smtpMessage.Body);
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
                var res = await client.SendAsync(email);

                await client.DisconnectAsync(true);

                Log.Information("Smtp result: {res}", res);
                return res != null ? true : false;
            }
        }
    }
}
