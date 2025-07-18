using CairaEdu.Core.ViewModels;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Account
{
	public class LoginModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		public LoginModel (SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}
		
		[BindProperty]
		public LoginViewModel Input { get; set; } 

		public async Task<IActionResult> OnPostAsync()
		{
			var result = await _signInManager.PasswordSignInAsync(Input.Correo, Input.Password, isPersistent:false, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(Input.Correo);
				var roles = await _userManager.GetRolesAsync(user);

				if (roles.Contains("Administrador"))
					return RedirectToPage("/Admin/IndexAdm");

				if (roles.Contains("Docente"))
					return RedirectToPage("/Index", new { area = "Docente" });

				if (roles.Contains("Estudiante"))
					return RedirectToPage("/Index", new { area = "Estudiante" });

				if (roles.Contains("Representante"))
					return RedirectToPage("/Index", new { area = "Representante" });

				return RedirectToPage("/Index");
			}

			ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido.");
			return Page();
		}
	}
}
