using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Docente
{
    [Authorize(Roles = "Administrador,Docente")]
    public class IndexDocModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
