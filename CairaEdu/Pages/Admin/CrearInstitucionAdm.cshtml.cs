using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class InstitucionModel : PageModel
    {
        public void OnGet()
        {
        }


    }
}
