using CairaEdu.Core.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace CairaEdu.Core.Services
{
	public class EmailService:IEmailService
	{
		private readonly IConfiguration _config;
		
		public EmailService(IConfiguration configuration)
		{
			_config = configuration;
		}

		public async Task EnviarEmailAsync(string para, string asunto, string mensaje)
		{
			var smtp = new SmtpClient(_config["Email:Smtp"], int.Parse(_config["Email:Port"]))
			{
				Credentials = new NetworkCredential(
				_config["Email:User"],
				_config["Email:Pass"]
			),
				EnableSsl = true
			};
			var mail = new MailMessage
			{
				From = new MailAddress(_config["Email:User"], "Caira"),
				Subject = asunto,
				Body = mensaje,
				IsBodyHtml = true
			};

			mail.To.Add(para);
			await smtp.SendMailAsync(mail);

		}
	}
}
