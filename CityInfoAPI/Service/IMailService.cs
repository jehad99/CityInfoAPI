namespace CityInfoAPI.Service
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}