using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class EditarMateriaAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditarMateriaAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

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

        public async Task<IActionResult> OnGetAsync()
        {
            var materia = await _context.Materias.FindAsync(Id);
            if (materia == null)
            {
                TempData["ErrorMessage"] = "Materia no encontrada.";
                return NotFound();
            }

            var profesorAsignado = _context.MateriaProfesores.FirstOrDefault(mp => mp.MateriaId == Id)?.UserId;

            Input = new InputModel
            {
                Nombre = materia.Nombre,
                Competencias = materia.Competencias,
                Objetivos = materia.Objetivos,
                Estado = materia.Estado,
                ProfesorId = profesorAsignado,
                LogoPath = materia.Imagen
            };

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // recargar Profesores
                return Page();
            }

            var materia = await _context.Materias.FindAsync(Id);
            if (materia == null)
            {
                return NotFound();
            }

            if (Input.Imagen != null && Input.Imagen.Length > 0)
            {
                if (Input.Imagen.Length > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("Input.Imagen", "El archivo no puede pesar más de 1MB.");
                    TempData["ErrorMessage"] = "El archivo no puede pesar más de 1MB.";
                    return Page();
                }

                var permittedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                if (!permittedTypes.Contains(Input.Imagen.ContentType))
                {
                    ModelState.AddModelError("Input.Imagen", "Solo se permiten imágenes PNG, JPG o JPEG.");
                    TempData["ErrorMessage"] = "Solo se permiten imágenes PNG, JPG o JPEG.";
                    return Page();
                }

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(Input.Imagen.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Input.Imagen", "La extensión del archivo no es válida.");
                    TempData["ErrorMessage"] = "La extensión del archivo no es válida.";
                    return Page();
                }

                if (ModelState.IsValid)
                {
                    var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "materias");
                    Directory.CreateDirectory(carpetaDestino);

                    var nombreArchivo = Guid.NewGuid().ToString() + extension;
                    var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                    using (var stream = new FileStream(rutaFisica, FileMode.Create))
                    {
                        await Input.Imagen.CopyToAsync(stream);
                    }

                    materia.Imagen = $"/img/materias/{nombreArchivo}";
                }
            }

            // Actualizar datos básicos
            materia.Nombre = Input.Nombre;
            materia.Competencias = Input.Competencias;
            materia.Objetivos = Input.Objetivos;
            materia.Estado = Input.Estado;

            // Actualizar relación con profesor
            var actualRelacion = _context.MateriaProfesores.FirstOrDefault(mp => mp.MateriaId == Id);
            if (actualRelacion != null)
            {
                _context.MateriaProfesores.Remove(actualRelacion);
            }

            if (!string.IsNullOrWhiteSpace(Input.ProfesorId))
            {
                var nuevaRelacion = new MateriaProfesor
                {
                    MateriaId = Id,
                    UserId = Input.ProfesorId
                };
                _context.MateriaProfesores.Add(nuevaRelacion);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Materia actualizada correctamente.";
            return RedirectToPage("VerMateriaAdm");
        }
    }
}
