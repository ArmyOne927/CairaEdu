using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Areas.Admin.Pages
{
	[Authorize(Roles ="Administrador")]
	public class IndexModel : PageModel
    {
        

        public IndexModel()
        {
            
        }

        public void OnGet()
        {

        }
    }
}
