namespace Domain.Models.Configuration
{
    public class SmtpConfiguration
    {
        public From From { get; set; }
        public string To { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class From
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
