using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class VerMateriaAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerMateriaAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Materia> Materias { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            // Obtener el usuario actual con su InstitucionId
            var usuarioActual = await _userManager.FindByIdAsync(userId);
            if (usuarioActual == null || usuarioActual.InstitucionId == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado o sin institución asignada.";
                return RedirectToPage("/Admin/VerInstitucionAdm");
            }
            var institucionId = user?.InstitucionId;

            Materias = _context.Materias
                .Where(m => m.InstitucionId == institucionId && m.Estado == "A")
                .OrderBy(m => m.Nombre)
                .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null)
            {
                materia.Estado = "I"; // Cambiar a Inactivo
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Materia eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró la materia que deseas eliminar.";
            }

            return RedirectToPage("/Admin/VerMateriaAdm");
        }

    }

}
