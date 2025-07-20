using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class EditarCicloAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditarCicloAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Region { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Today;

        [BindProperty]
        public List<PeriodoInput> Periodos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var ciclo = await _context.CiclosLectivos
                .Include(c => c.Periodos)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (ciclo == null)
            {
                TempData["ErrorMessage"] = "No se encontró el ciclo lectivo.";
                return RedirectToPage("VerCiclosAdm");
            }

            Nombre = ciclo.Nombre;
            Region = ciclo.Region;
            FechaInicio = ciclo.FechaInicio;
            FechaFin = ciclo.FechaFin;

            Periodos = ciclo.Periodos.Select(p => new PeriodoInput
            {
                Nombre = p.Nombre,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FechaFin <= FechaInicio)
            {
                TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                return Page();
            }

            if (Periodos == null || !Periodos.Any())
            {
                TempData["ErrorMessage"] = "Debe agregar al menos un periodo.";
                return Page();
            }

            foreach (var periodo in Periodos)
            {
                if (periodo.FechaFin <= periodo.FechaInicio ||
                    periodo.FechaInicio < FechaInicio || periodo.FechaFin > FechaFin)
                {
                    TempData["ErrorMessage"] = $"Periodo '{periodo.Nombre}' inválido.";
                    return Page();
                }
            }

            var ciclo = await _context.CiclosLectivos.Include(c => c.Periodos).FirstOrDefaultAsync(c => c.Id == Id);
            if (ciclo == null)
            {
                TempData["ErrorMessage"] = "No se encontró el ciclo a editar.";
                return RedirectToPage("VerCiclosAdm");
            }

            ciclo.Nombre = Nombre;
            ciclo.Region = Region;
            ciclo.FechaInicio = FechaInicio;
            ciclo.FechaFin = FechaFin;

            // Reemplazar periodos existentes
            _context.Periodos.RemoveRange(ciclo.Periodos);
            ciclo.Periodos = Periodos.Select(p => new Periodo
            {
                Nombre = p.Nombre,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                EsActivo = true,
                Tipo = ""
            }).ToList();

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Ciclo lectivo actualizado correctamente.";
            return RedirectToPage("VerCiclosAdm");
        }

        public class PeriodoInput
        {
            public required string Nombre { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
        }
    }
}
