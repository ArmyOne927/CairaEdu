using Microsoft.AspNetCore.Mvc;

namespace CairaEdu.Core.Interfaces
{
	public interface IEmailService
	{
		Task EnviarEmailAsync(string destinatario, string assunto, string mensagem);
	}
}
