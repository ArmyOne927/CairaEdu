using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class VerCiclosAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerCiclosAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CicloLectivo> Ciclos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.InstitucionId == null)
            {
                TempData["ErrorMessage"] = "No se pudo determinar la institución del usuario.";
                return RedirectToPage("VerInstitucionAdm");
            }

            Ciclos = await _context.CiclosLectivos
                .Where(c => c.Estado == true && c.InstitucionId == user.InstitucionId)
                .Include(c => c.Periodos)
                .OrderByDescending(c => c.FechaInicio)
                .ToListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var ciclo = await _context.CiclosLectivos.FindAsync(id);
            if (ciclo != null)
            {
                ciclo.Estado = false; // Cambiar el estado booleano a false para marcar como eliminado
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ciclo eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el ciclo que deseas eliminar.";
            }

            return RedirectToPage("/Admin/VerCiclosAdm");
        }
    }
}
