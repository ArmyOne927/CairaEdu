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
		public RecoverModel(UserManager<ApplicationUser> userManager,IEmailService emailService)
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
			var res=await _userManager.FindByEmailAsync(Input.Email);
			if (res == null) {
				ModelState.AddModelError("Input.Email", "Este correo no está en el sistema.");
				return Page();
			}
			
			TempData["SuccessMessage"] = "Se envio una nueva contrasena a tu correo.";
			await _emailService.EnviarEmailAsync(Input.Email, "Tu nuevo password", $"Tu nueva contraseña es: <strong>@Jonaiker13</strong>");
			return RedirectToPage("/Account/Recover");

		}
	}
}
