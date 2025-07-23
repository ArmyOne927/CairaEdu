using CairaEdu.Core.ViewModels;
using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Pages.Admin
{
    public class EditarParaleloAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditarParaleloAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EditarParaleloViewModel ParaleloVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var paralelo = await _context.Paralelos
                .Include(p => p.Estudiantes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paralelo == null)
                return NotFound();

            // 1. Usuario logueado
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var usuarioActual = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuarioActual == null || usuarioActual.InstitucionId == null)
                return Forbid();

            var institucionId = usuarioActual.InstitucionId;

            // 2. Todos los estudiantes con rol "Estudiante"
            var estudiantes = await _userManager.GetUsersInRoleAsync("Estudiante");

            // 3. Obtener IDs de estudiantes asignados a *otros* paralelos
            var idsAsignadosAOtrosParalelos = await _context.EstudiantesXParalelo
                .Where(e => e.ParaleloId != paralelo.Id)
                .Select(e => e.EstudianteId)
                .ToListAsync();


            // 4. Filtrar estudiantes de la misma institución que no estén en otros paralelos
            var estudiantesFiltrados = estudiantes
                .Where(e => e.InstitucionId == institucionId && !idsAsignadosAOtrosParalelos.Contains(e.Id))
                .ToList();

            // 5. Preparar ViewModel con estudiantes NO asignados a ningún paralelo
            ParaleloVM = new EditarParaleloViewModel
            {
                ParaleloId = paralelo.Id,
                Nombre = paralelo.Nombre,
                EstudiantesSeleccionados = paralelo.Estudiantes.Select(e => e.EstudianteId).ToList(),
                ListaEstudiantes = estudiantesFiltrados.Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = $"{e.Nombres} {e.Apellidos}"
                }).ToList()
            };

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Algo anda mal, revisa los datos";
                return Page(); 
            }

            var paralelo = await _context.Paralelos
                .Include(p => p.Estudiantes)
                .FirstOrDefaultAsync(p => p.Id == ParaleloVM.ParaleloId);

            if (paralelo == null) 
            {
                TempData["ErrorMessage"] = "No se recibió el ID del paralelo";
                return RedirectToPage("VerCursosAdm");
            }

            paralelo.Nombre = ParaleloVM.Nombre;

            // Eliminar asignaciones actuales
            _context.EstudiantesXParalelo.RemoveRange(paralelo.Estudiantes);

            // Agregar nuevas asignaciones
            foreach (var estudianteId in ParaleloVM.EstudiantesSeleccionados)
            {
                _context.EstudiantesXParalelo.Add(new EstudianteXParalelo
                {
                    EstudianteId = estudianteId,
                    ParaleloId = paralelo.Id
                });
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Paralelo actualizado correctamente.";
            return RedirectToPage("VerCursosAdm"); 
        }
    }
}
