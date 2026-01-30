using Domain.Interfaces;
using Resend;

namespace Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IResend _resend;
        private readonly string _mailFrom;
        private readonly string _mailTo;

        public MailService(IResend resend)
        {
            _resend = resend;
            _mailFrom = Environment.GetEnvironmentVariable( "MAIL_FROM" )!;
            _mailTo = Environment.GetEnvironmentVariable( "MAIL_TO" )!;
        }

        public async Task SendFirstContact(string name, string email, string message)
        {
            var subject = $"Contacto desde tu Portfolio de {name}";

            var htmlBody = $@"
                        <strong>Nuevo contacto desde tu portfolio</strong><br/><br/>
                        <b>Nombre:</b> {name}<br/>
                        <b>Email:</b> {email}<br/><br/>
                        <b>Mensaje:</b><br/>{message}";

            var mail = new EmailMessage
            {
                From = _mailFrom,
                Subject = subject,
                HtmlBody = htmlBody
            };

            mail.To.Add(_mailTo);

            await _resend.EmailSendAsync(mail);
        }
    }
}
