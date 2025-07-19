using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CairaEdu.Data.Identity;

namespace CairaEdu.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();

            // Eliminar cookie manualmente (opcional)
            Response.Cookies.Delete(".AspNetCore.Identity.Application");

            return RedirectToPage("/Account/Login"); // o Redirect("/")
        }
    }
}
