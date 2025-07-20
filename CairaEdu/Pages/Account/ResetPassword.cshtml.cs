using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string UserId { get; set; }

            [Required]
            public string Token { get; set; }

            [Required(ErrorMessage = "Ingrese la nueva contrase�a")]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "La contrase�a tiene que tener una May�scula, un n�mer0 y S & mbolo.Por ejemplo: Mario13*", MinimumLength = 6)]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirme la contrase�a")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "Las contrase�as no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Enlace inv�lido o expirado.";
                return RedirectToPage("/Account/Login");
            }

            Input = new InputModel
            {
                UserId = userId,
                Token = token
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToPage("/Account/Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Tu contrase�a ha sido restablecida correctamente.";
                return RedirectToPage("/Account/Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}
