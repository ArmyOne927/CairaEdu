using CairaEdu.Core.ViewModels;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Account
{
    public class RegisterModel : PageModel
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		public RegisterModel(UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		[BindProperty]
		public RegisterViewModel Input { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			
			if (!ModelState.IsValid)
			{
				return Page();
			}
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Documento = Input.Documento,
                TipoDocumentoId = Input.TipoDocumentoId
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
			if (result.Succeeded)
			{
				if (!await _roleManager.RoleExistsAsync(Input.Role))
				{
					await _roleManager.CreateAsync(new ApplicationRole { Name = Input.Role });
				}
				await _userManager.AddToRoleAsync(user, "Administrador");
				await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Account/Login");

            }
            foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return Page();
		}
	}
}
