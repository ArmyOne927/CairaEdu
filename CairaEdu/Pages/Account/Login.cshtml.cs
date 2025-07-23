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

		[BindProperty]
		public LoginViewModel Input { get; set; } = new LoginViewModel(); // Inicialización para evitar valores NULL

		public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<IActionResult> OnPostAsync()
		{
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Correo);

			if (user == null)
			{
                TempData["Error"] = "Usuario no encontrado";
                return Page();
            }

            // Verificar si la cuenta está bloqueada
            if (await _userManager.IsLockedOutAsync(user))
            {
                TempData["Error"] = "Tu cuenta está bloqueada temporalmente. Intenta más tarde.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Correo, Input.Password, isPersistent: false, lockoutOnFailure: false);

			if (result.Succeeded)
			{
                // Reiniciar contador de fallos
                await _userManager.ResetAccessFailedCountAsync(user);

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrador"))
                { 
                    TempData["SuccessMessage"] = "Bienvenido.";
                     return RedirectToPage("/Admin/IndexAdm");
                }

                if (roles.Contains("Docente")) { 
                    TempData["SuccessMessage"] = "Bienvenido.";
                    return RedirectToPage("/Docente/IndexDoc");
                }

                if (roles.Contains("Estudiante")) { 
                    TempData["SuccessMessage"] = "Bienvenido.";
                    return RedirectToPage("/Estudiante/IndexEst");
                }

                if (roles.Contains("Representante")) { 
                    TempData["SuccessMessage"] = "Bienvenido.";
                    return RedirectToPage("Representante/IndexRep");
                }

                return RedirectToPage("Account/Login");
            }

            // Si fue fallido
            await _userManager.AccessFailedAsync(user); // Aumenta el contador manualmente

            if (await _userManager.IsLockedOutAsync(user))
            {
                TempData["Error"]= "Has excedido el número de intentos. Tu cuenta estará bloqueada por 30 segundos.";
                return Page();
            }

            int maxIntentos = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            int intentosFallidos = await _userManager.GetAccessFailedCountAsync(user);
            int restantes = maxIntentos - intentosFallidos;

            TempData["Warning"] = $"Contraseña incorrecta. Te quedan {restantes} intento(s) antes del bloqueo.";
            return Page();
        }
	}
}
