namespace GA_TEST.Services
{
    public interface ILocalMailService
    {
        void Send(string subject, string message);
    }
}