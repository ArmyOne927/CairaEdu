using CairaEdu.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages
{
    public class TestCorreoModel : PageModel
    {
        private readonly IEmailService _emailService;

        public TestCorreoModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _emailService.EnviarEmailAsync(
                "caira.edu.soporte@gmail.com",
                "Prueba desde CairaEdu",
                "<strong>Este es un mensaje de prueba exitoso</strong>");

            TempData["Success"] = "Correo enviado correctamente";
            return Page();
        }
    }

}
