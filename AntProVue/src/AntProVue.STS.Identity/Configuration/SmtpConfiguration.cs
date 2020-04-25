namespace AntProVue.STS.Identity.Configuration
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 25; // default smtp port
        public bool UseSSL { get; set; } = true;
    }
}






