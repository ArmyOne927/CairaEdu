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

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class EditarCursoAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditarCursoAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CursoInputModel CursoInput { get; set; }

        public List<SelectListItem> CiclosLectivos { get; set; }

        public class CursoInputModel
        {
            public int Id { get; set; }

            [Required]
            public string Nombre { get; set; }

            [Required]
            public int CicloLectivoId { get; set; }

            public List<ParaleloInputModel> Paralelos { get; set; } = new();
        }

        public class ParaleloInputModel
        {
            public int Id { get; set; }

            [Required]
            public string Nombre { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Paralelos.Where(p => p.Estado == 'A'))
                .FirstOrDefaultAsync(c => c.Id == id && c.Estado == 'A');

            if (curso == null)
            {
                TempData["ErrorMessage"] = "Curso no encontrado.";
                return RedirectToPage("/Admin/VerCursosAdm");
            }

            var user = await _userManager.GetUserAsync(User);

            CursoInput = new CursoInputModel
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                CicloLectivoId = curso.CicloLectivoId,
                Paralelos = curso.Paralelos.Select(p => new ParaleloInputModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList()
            };

            CiclosLectivos = _context.CiclosLectivos
                .Where(c => c.InstitucionId == user.InstitucionId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} - {c.Region} ({c.FechaInicio:dd/MM/yyyy})"
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarCiclosAsync();
                TempData["ErrorMessage"] = "Revisa los campos obligatorios.";
                return Page();
            }

            var curso = await _context.Cursos
                .Include(c => c.Paralelos)
                .FirstOrDefaultAsync(c => c.Id == CursoInput.Id);

            if (curso == null)
            {
                TempData["ErrorMessage"] = "Curso no encontrado.";
                return RedirectToPage("/Admin/VerCursosAdm");
            }

            curso.Nombre = CursoInput.Nombre;
            curso.CicloLectivoId = CursoInput.CicloLectivoId;

            // Recoge los IDs de paralelos activos desde el formulario
            var paralelosForm = CursoInput.Paralelos ?? new();
            var idsForm = paralelosForm.Where(p => !string.IsNullOrWhiteSpace(p.Nombre)).Select(p => p.Id).ToList();

            // Marcar como inactivos los que no están
            foreach (var paraleloBD in curso.Paralelos)
            {
                if (!idsForm.Contains(paraleloBD.Id))
                    paraleloBD.Estado = 'I';
            }

            // Nuevos o actualizados
            foreach (var p in paralelosForm)
            {
                var nombre = p.Nombre?.Trim();
                if (string.IsNullOrWhiteSpace(nombre)) continue;

                if (p.Id == 0)
                {
                    curso.Paralelos.Add(new Paralelo { Nombre = nombre, Estado = 'A' });
                }
                else
                {
                    var existente = curso.Paralelos.FirstOrDefault(x => x.Id == p.Id);
                    if (existente != null)
                    {
                        existente.Nombre = nombre;
                        existente.Estado = 'A';
                    }
                }
            }

            await _context.SaveChangesAsync();

            if (!curso.Paralelos.Any(p => p.Estado == 'A'))
            {
                TempData["InfoMessage"] = "Curso actualizado. Actualmente no tiene paralelos activos.";
                await CargarCiclosAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Curso actualizado correctamente.";
            return RedirectToPage("/Admin/VerCursosAdm");
        }






        public async Task<IActionResult> OnPostEliminarParaleloAsync(int id)
        {
            var paralelo = await _context.Paralelos.FindAsync(id);
            if (paralelo == null)
            {
                TempData["ErrorMessage"] = "Paralelo no encontrado.";
                return RedirectToPage("/Admin/VerCursosAdm");
            }

            paralelo.Estado = 'I';
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Paralelo eliminado correctamente.";

            return RedirectToPage("EditarCursoAdm", new { id = paralelo.CursoId });

        }

        private async Task CargarCiclosAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CiclosLectivos = _context.CiclosLectivos
                .Where(c => c.InstitucionId == user.InstitucionId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} - {c.Region} ({c.FechaInicio:dd/MM/yyyy})"
                }).ToList();
        }
    }
}
