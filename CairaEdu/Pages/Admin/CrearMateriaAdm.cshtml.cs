using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    public class CrearMateriaAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrearMateriaAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> Profesores { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Nombre { get; set; }

            public string? Competencias { get; set; }

            public string? Objetivos { get; set; }

            public string Estado { get; set; }

            public string? ProfesorId { get; set; }

            [Display(Name = "Imagen Materia")]
            public IFormFile? Imagen { get; set; }
            public string? LogoPath { get; set; }
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = await _userManager.FindByIdAsync(userId);
            var institucionId = usuario?.InstitucionId;

            Profesores = (await _userManager.GetUsersInRoleAsync("Docente"))
                .Where(p => p.InstitucionId == institucionId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id,
                    Text = $"{p.Nombres} {p.Apellidos}"
                }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Recargar combos si hay error
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = await _userManager.FindByIdAsync(userId);
            var institucionId = usuario?.InstitucionId;

            if (institucionId == null)
            {
                ModelState.AddModelError(string.Empty, "El usuario no tiene una institución asociada.");
                await OnGetAsync();
                return Page();
            }

            string? rutaImagen = null;

            if (Input.Imagen != null && Input.Imagen.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "materias");
                Directory.CreateDirectory(carpetaDestino); // Asegura que exista la carpeta

                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(Input.Imagen.FileName);
                var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await Input.Imagen.CopyToAsync(stream);
                }

                rutaImagen = $"/img/materias/{nombreArchivo}";
                Input.LogoPath = rutaImagen; // para que se renderice si falla validación
            }


            var materia = new Materia
            {
                Nombre = Input.Nombre,
                Competencias = Input.Competencias,
                Objetivos = Input.Objetivos,
                Estado = Input.Estado,
                Imagen = rutaImagen,
                InstitucionId = institucionId.Value
            };

            _context.Materias.Add(materia);

            if (!string.IsNullOrWhiteSpace(Input.ProfesorId)){ 
                var relacion = new MateriaProfesor
                {
                    Materia = materia,
                    UserId = Input.ProfesorId
                };
                _context.Set<MateriaProfesor>().Add(relacion);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("VerMateriaAdm");
        }
    }
}
