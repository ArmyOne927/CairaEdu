using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Areas.Docente.Pages
{
	[Authorize(Roles = "Docente")]
	public class IndexModel : PageModel
    {
       

        public IndexModel()
        {
            
        }

        public void OnGet()
        {
			foreach (var claim in User.Claims)
			{
				Console.WriteLine($"Tipo: {claim.Type}, Valor: {claim.Value}");
			}
		}
    }
}
