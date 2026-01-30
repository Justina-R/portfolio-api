using Domain.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly string _mailFrom;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public MailService()
        {
            _mailFrom = Environment.GetEnvironmentVariable("SMTP_USER")!;
            _smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")!;
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            _smtpUser = Environment.GetEnvironmentVariable("SMTP_USER")!;
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS")!;
        }

        public async Task SendFirstContact(string name, string email, string message)
        {
            var subject = $"Contacto desde tu Portfolio de {name}";
            var body = $"{name} quiere contactarse contigo.\n\nEmail: {email}\nMensaje: {message}";

            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
                Timeout = 10000
            };

            var mail = new MailMessage(_mailFrom, _mailFrom, subject, body);

            await client.SendMailAsync(mail);
        }
    }
}
