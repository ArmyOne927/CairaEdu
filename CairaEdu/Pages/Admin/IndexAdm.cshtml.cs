using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
	public class IndexAdmModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
