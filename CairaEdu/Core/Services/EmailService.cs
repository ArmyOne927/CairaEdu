using CairaEdu.Core.Configuration;
using CairaEdu.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CairaEdu.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task EnviarEmailAsync(string destino, string asunto, string mensajeHtml)
        {
            using var mensaje = new MailMessage
            {
                From = new MailAddress(_settings.User, "CairaEdu"),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true
            };

            mensaje.To.Add(destino);

            using var smtp = new SmtpClient(_settings.Smtp, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Pass),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mensaje);
        }
    }


}
