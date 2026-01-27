using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
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


        public MailService(IConfiguration configuration)
        {
            _mailFrom = configuration["mailSettings:mailFromAddress"];
            _smtpHost = configuration["mailSettings:host"];
            _smtpPort = int.Parse(configuration["mailSettings:port"] ?? "587");
            _smtpUser = configuration["mailSettings:username"];
            _smtpPass = configuration["mailSettings:password"];
        }

        public async Task SendFirstContact(string name, string email, string message)
        {
            string subject = $"Contacto desde tu Portfolio de {name}";
            string body = $"{name} quiere contactarse contigo desde tu Portfolio.\n" +
                        $"Fecha: {DateTime.Now}\n" +
                        $"Email: {email}\n" +
                        $"Mensaje: {message}";

            try
            {
                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                var mail = new MailMessage(_mailFrom, _mailFrom, subject, body);
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando mail: {ex.Message}");
                throw;
            }
        }
    }
}