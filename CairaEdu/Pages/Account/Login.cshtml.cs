using CairaEdu.Data.Identity;
using CairaEdu.Migrations;
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
		



		public async Task<IActionResult> OnPostAsync()
		{
			var result = await _signInManager.PasswordSignInAsync("riordan1783@gmail.com", "@Jonaiker13", isPersistent:false, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync("riordan1783@gmail.com");
				var roles = await _userManager.GetRolesAsync(user);

				if (roles.Contains("Administrador"))
					return RedirectToPage("/Index", new { area = "Admin" });

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
