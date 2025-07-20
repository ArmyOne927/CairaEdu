using CairaEdu.Core.Interfaces;
using CairaEdu.Core.ViewModels;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Account
{
    public class RecoverModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public RecoverModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public RecoverViewModel Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var res = await _userManager.FindByEmailAsync(Input.Correo);
            if (res == null)
            {
                ModelState.AddModelError("Input.Correo", "Este correo no está en nuestros registros.");
                TempData["Error"] = "Este correo no está en nuestros registros.";
                return Page();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(res);
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { userId = res.Id, token = token },
                Request.Scheme
            );

            var mensaje = $@"
		<p>Hola,</p>
		<p>Solicitaste restablecer tu contraseña. Haz clic en el siguiente enlace para establecer una nueva:</p>
		<p><a href='{callbackUrl}'>Restablecer contraseña</a></p>
		<p>Si no solicitaste este cambio, puedes ignorar este correo.</p>";

            await _emailService.EnviarEmailAsync(Input.Correo, "Recuperación de contraseña - CairaEdu", mensaje);

            TempData["Success"] = "Se ha enviado un enlace de recuperación a tu correo.";
            return RedirectToPage("/Account/Login");
        }
    }
}
