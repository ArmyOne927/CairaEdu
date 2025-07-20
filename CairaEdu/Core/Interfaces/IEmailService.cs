namespace CairaEdu.Core.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destino, string asunto, string mensajeHtml);
    }
}
