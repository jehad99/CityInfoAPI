namespace CityInfoAPI.Service
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "Jehad.hashish.1999@gmail.com";
        private string _mailFrom = "noreply@myCompany.com";

        public LocalMailService(IConfiguration config)
        {
            _mailTo = config["mailSettings:mailToAddress"];
            _mailFrom = config["mailSettings:mailFromAddress"];
        }

        public IConfiguration Config { get; }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail From {_mailFrom} To {_mailTo}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
