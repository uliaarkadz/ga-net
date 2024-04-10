using System;
namespace GA_TEST.Services
{
	public class CloudMailService : ILocalMailService
    {
        private string _mailTo = "juliabuiko@gmail.com";
        private string _mailFrom = "no-reply@gmail.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"Message {message}");
        }

    }
}

