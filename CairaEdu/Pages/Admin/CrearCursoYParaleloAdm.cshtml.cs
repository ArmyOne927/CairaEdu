using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CrearCursoYParaleloAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrearCursoYParaleloAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CursoInputModel CursoInput { get; set; }

        public List<SelectListItem> CiclosLectivos { get; set; }

        public class CursoInputModel
        {
            [Required]
            public string Nombre { get; set; }

            [Required]
            public int CicloLectivoId { get; set; }

            public List<ParaleloInputModel> Paralelos { get; set; } = new();
        }

        public class ParaleloInputModel
        {
            [Required]
            public string Nombre { get; set; }
        }

        public void OnGet()
        {
            CargarCiclos();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CargarCiclos();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Revise los datos ingresados.";
                return Page();
            }

            if (CursoInput.Paralelos == null || !CursoInput.Paralelos.Any())
            {
                TempData["ErrorMessage"] = "Debe ingresar al menos un paralelo.";
                return Page();
            }

            bool cursoExiste = await _context.Cursos.AnyAsync(c =>
                c.Nombre == CursoInput.Nombre &&
                c.CicloLectivoId == CursoInput.CicloLectivoId);

            if (cursoExiste)
            {
                TempData["ErrorMessage"] = "Ya existe un curso con ese nombre en el ciclo lectivo seleccionado.";
                return Page();
            }

            // Validar paralelos duplicados
            var nombres = CursoInput.Paralelos.Select(p => p.Nombre.Trim().ToUpper()).ToList();
            if (nombres.Distinct().Count() != nombres.Count)
            {
                TempData["ErrorMessage"] = "Los nombres de los paralelos no pueden repetirse.";
                return Page();
            }

            var nuevoCurso = new Curso
            {
                Nombre = CursoInput.Nombre,
                CicloLectivoId = CursoInput.CicloLectivoId,
                Paralelos = CursoInput.Paralelos.Select(p => new Paralelo
                {
                    Nombre = p.Nombre.Trim()
                }).ToList()
            };

            _context.Cursos.Add(nuevoCurso);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Curso y paralelos creados correctamente.";
            return RedirectToPage("VerCursosAdm");
        }

        private void CargarCiclos()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user?.InstitucionId == null)
            {
                CiclosLectivos = new List<SelectListItem>();
                TempData["ErrorMessage"] = "No se pudo determinar la institución del usuario.";
                return;
            }

            CiclosLectivos = _context.CiclosLectivos
                .Where(c => c.InstitucionId == user.InstitucionId)
                .OrderByDescending(c => c.FechaInicio)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} - {c.Region} ({c.FechaInicio:dd/MM/yyyy})"
                }).ToList();
        }

    }


}