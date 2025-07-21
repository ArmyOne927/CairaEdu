using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CairaEdu.Data.Identity;

namespace CairaEdu.Pages.Admin
{
    public class VerCursosAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerCursosAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CursoConParalelosViewModel> CursosConParalelos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.InstitucionId == null)
            {
                TempData["ErrorMessage"] = "No se pudo determinar la institución del usuario.";
                CursosConParalelos = new List<CursoConParalelosViewModel>();
                return RedirectToPage("VerInstitucionAdm");
            }

            CursosConParalelos = await _context.Cursos
                .Include(c => c.CicloLectivo)
                .Include(c => c.Paralelos)
                .Where(c => c.Estado =='A' && c.CicloLectivo.InstitucionId == user.InstitucionId)
                .OrderBy(c => c.CicloLectivo.FechaInicio)
                .ThenBy(c => c.Nombre)
                .Select(c => new CursoConParalelosViewModel
                {
                    Curso = c,
                    Paralelos = c.Paralelos
                    .Where(p => p.Estado == 'A')
                    .OrderBy(p => p.Nombre).ToList()
                })
                .ToListAsync();

            return Page();
        }


        public class CursoConParalelosViewModel
        {
            public Curso Curso { get; set; }
            public List<Paralelo> Paralelos { get; set; }
        }

        public async Task<IActionResult> OnPostEliminarCursoAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                TempData["ErrorMessage"] = "Curso no encontrado.";
                return RedirectToPage();
            }

            curso.Estado = 'I';
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Curso eliminado correctamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarParaleloAsync(int id)
        {
            var paralelo = await _context.Paralelos.FindAsync(id);

            if (paralelo == null)
            {
                TempData["ErrorMessage"] = "Paralelo no encontrado.";
                return RedirectToPage();
            }

            paralelo.Estado = 'I';
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Paralelo eliminado correctamente.";
            return RedirectToPage();
        }


    }

}
