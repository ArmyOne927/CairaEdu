using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CrearInstitucionAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrearInstitucionAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public string Nombre { get; set; }
            public string Direccion { get; set; }
            public string Dominio { get; set; }
            public string Ruc { get; set; }
            public string? Telefono { get; set; }
            public int CiudadId { get; set; }
            public char Estado { get; set; }
            public IFormFile? Logo { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            byte[]? logoBytes;

            // Validaciones del archivo si existe
            if (Input.Logo != null)
            {
                if (Input.Logo.Length > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("Input.Logo", "El archivo no puede pesar más de 1MB.");
                    TempData["ErrorMessage"] = "El archivo no puede pesar más de 1MB.";
                    return Page();
                }

                var permittedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                if (!permittedTypes.Contains(Input.Logo.ContentType))
                {
                    ModelState.AddModelError("Input.Logo", "Solo se permiten imágenes PNG, jpg o JPEG.");
                    TempData["ErrorMessage"] = "Solo se permiten imágenes PNG, JPG o JPEG.";
                    return Page();
                }

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(Input.Logo.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Input.Logo", "La extensión del archivo no es válida.");
                    TempData["ErrorMessage"] = "La extensión del archivo no es válida.";
                    return Page(); 
                }
            }

            // Verificar errores de validación general
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errores)
                    Console.WriteLine(error);
                TempData["ErrorMessage"] = "Por favor corrige los errores antes de continuar.";
                return Page();
            }

            // Procesamiento de imagen
            if (Input.Logo != null)
            {
                using var memoryStream = new MemoryStream();
                await Input.Logo.CopyToAsync(memoryStream);
                logoBytes = memoryStream.ToArray();
            }
            else
            {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "logoDefaultInst.png");
                Console.WriteLine($"Archivo recibido: {Input.Logo != null}, Tamaño: {Input.Logo?.Length ?? 0}");
                logoBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
            }

            // Guardar la institución
            var institucion = new Institucion
            {
                Nombre = Input.Nombre,
                Direccion = Input.Direccion,
                Dominio = Input.Dominio,
                Ruc = Input.Ruc,
                Telefono = Input.Telefono,
                CiudadId = Input.CiudadId,
                Estado = Input.Estado,
                Logo = logoBytes
            };

            _context.Instituciones.Add(institucion);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Institución creada y vinculada correctamente.";

            // Actualizar usuario autenticado
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario != null)
            {
                usuario.InstitucionId = institucion.Id;
                await _userManager.UpdateAsync(usuario);
            }

            return RedirectToPage("/Admin/VerInstitucionAdm");
        }

        public List<SelectListItem> Provincias { get; set; } = new();

        public void OnGet()
        {
            Provincias = _context.Provincias
                .Where(p => p.Estado == 'A')
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                })
                .ToList();
        }

        public List<SelectListItem> Ciudades { get; set; } = new();
        // using Microsoft.AspNetCore.Mvc;
        public JsonResult OnGetCiudadesPorProvincia(int provinciaId)
        {
            var ciudades = _context.Ciudades
                .Where(c => c.ProvinciaId == provinciaId)
                .Select(c => new { id=c.Id, nombre= c.Nombre })
                .ToList();
            return new JsonResult(ciudades);
        }
    }
}
