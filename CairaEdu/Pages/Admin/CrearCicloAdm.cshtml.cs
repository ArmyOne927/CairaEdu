using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
//gewrhewjhwthkwrjh
namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CrearCicloAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrearCicloAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Region { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }= DateTime.Today;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Today;

        [BindProperty]
        public List<PeriodoInput> Periodos { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FechaFin <= FechaInicio)
            {
                ModelState.AddModelError(string.Empty, "La fecha de fin debe ser posterior a la fecha de inicio.");
                TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                return Page();
            }

            if (Periodos == null || !Periodos.Any())
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un periodo.");
                TempData["ErrorMessage"] = "Debe agregar al menos un periodo.";
                return Page();
            }

            foreach (var periodo in Periodos)
            {
                if (periodo.FechaFin <= periodo.FechaInicio)
                {
                    ModelState.AddModelError(string.Empty, $"El periodo '{periodo.Nombre}' tiene una fecha inválida.");
                    TempData["ErrorMessage"] = $"El periodo '{periodo.Nombre}' tiene una fecha inválida.";
                    return Page();
                }

                if (periodo.FechaInicio < FechaInicio || periodo.FechaFin > FechaFin)
                {
                    ModelState.AddModelError(string.Empty, $"El periodo '{periodo.Nombre}' debe estar dentro del ciclo lectivo.");
                    TempData["ErrorMessage"] = $"El periodo '{periodo.Nombre}' debe estar dentro del ciclo lectivo.";
                    return Page();
                }
                if (periodo.Nombre == null)
                {
                    ModelState.AddModelError(string.Empty, "El nombre del periodo no puede estar vacío.");
                    TempData["ErrorMessage"] = "El nombre del periodo no puede estar vacío.";
                    return Page();
                }
            }

            // Obtener usuario actual
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.InstitucionId == 0)
            {
                TempData["ErrorMessage"] = "No se pudo determinar la institución del usuario.";
                return Page();
            }

            var ciclo = new CicloLectivo
            {
                Nombre = Nombre,
                Region = Region,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                Estado = true,
                FechaCreacion = DateTime.Now,
                InstitucionId = user.InstitucionId.Value, // Aquí se asigna automáticamente
                Periodos = Periodos.Select(p => new Periodo
                {
                    Nombre = p.Nombre,
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    EsActivo = true,
                    Tipo = ""
                }).ToList()
            };

            _context.CiclosLectivos.Add(ciclo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ciclo lectivo creado correctamente.";
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
