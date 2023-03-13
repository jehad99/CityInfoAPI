namespace CityInfoAPI.Service
{
    public class CloudMailService
    {
        private string _mailTo = "Jehad.hashish.1999@gmail.com";
        private string _mailFrom = "noreply@myCompany.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail From {_mailFrom} To {_mailTo}," + $"With {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
