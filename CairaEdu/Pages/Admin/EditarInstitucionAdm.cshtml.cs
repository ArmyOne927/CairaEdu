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
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class EditarInstitucionAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditarInstitucionAdmModel(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> Provincias { get; set; } = new();
        public List<SelectListItem> Ciudades { get; set; } = new();
        public string? LogoBase64 { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "El nombre de la institución es obligatorio.")]
            public string Nombre { get; set; } = "";
            [Required(ErrorMessage = "La dirección de la institución es obligatoria.")]
            public string Direccion { get; set; } = "";
            [Required(ErrorMessage = "El RUC de la institución es obligatorio.")]
            public string Ruc { get; set; } = "";

            public string Telefono { get; set; } = "";

            public string Dominio { get; set; } = "";
            [Required(ErrorMessage = "Debe seleccionar una provincia y una ciudad.")]
            public int CiudadId { get; set; }
            public IFormFile? Logo { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario?.InstitucionId == null) return NotFound();

            var institucion = await _context.Instituciones
                .Include(i => i.Ciudad)
                .ThenInclude(c => c.Provincia)
                .FirstOrDefaultAsync(i => i.Id == usuario.InstitucionId);

            if (institucion == null) return RedirectToPage("/Admin/VerInstitucionAdm");

            // Precargar input
            Input = new InputModel
            {
                Id = institucion.Id,
                Nombre = institucion.Nombre,
                Direccion = institucion.Direccion,
                Dominio = institucion.Dominio,
                Ruc = institucion.Ruc,
                Telefono = institucion.Telefono,
                CiudadId = institucion.CiudadId
            };

            LogoBase64 = institucion.Logo != null
                ? Convert.ToBase64String(institucion.Logo)
                : Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_env.WebRootPath, "img", "LogoDefaultInst.png")));

            await CargarProvinciasYCiudades(institucion.Ciudad?.ProvinciaId ?? 0);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            byte[]? logoBytes;
            bool logoInvalido = false ;

            // Validaciones del archivo si existe
            if (Input.Logo != null)
            {
                var huboError = false;
                if (Input.Logo.Length == 0)
                {
                    ModelState.AddModelError("Input.Logo", "El archivo no puede estar vacío.");
                    TempData["ErrorMessage"] = "El archivo no puede estar vacío.";
                    huboError = true;
                }
                if (Input.Logo.Length > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("Input.Logo", "El archivo no puede pesar más de 1MB.");
                    TempData["ErrorMessage"] = "El archivo no puede pesar más de 1MB.";
                    huboError = true;
                }

                var permittedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
                if (!permittedTypes.Contains(Input.Logo.ContentType))
                {
                    ModelState.AddModelError("Input.Logo", "Solo se permiten imágenes PNG, jpg o JPEG.");
                    TempData["ErrorMessage"] = "Solo se permiten imágenes PNG, JPG o JPEG.";
                    huboError = true;
                }

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(Input.Logo.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Input.Logo", "La extensión del archivo no es válida.");
                    TempData["ErrorMessage"] = "La extensión del archivo no es válida.";
                    huboError = true;
                }
                if (huboError)
                {
                    logoInvalido = true;
                    Console.WriteLine("Errores de validación:");           
                }
            }

            // Verificar errores de validación general
            if (!ModelState.IsValid)
            {
                
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errores)
                    Console.WriteLine(error);
                TempData["ErrorMessage"] = "Por favor corrige los errores antes de continuar.";
                await CargarProvinciasYCiudades(Input.CiudadId);
                return Page();
            }
            if (logoInvalido)
            {
                TempData["ErrorMessage"] = "Por favor corrige los errores antes de continuar.";
                await CargarProvinciasYCiudades(Input.CiudadId);
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
                logoBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
            }

            // Buscar la institución existente
            var institucion = await _context.Instituciones.FindAsync(Input.Id);
            if (institucion == null)
            {
                return NotFound();
            }

            // Actualizar los campos
            institucion.Nombre = Input.Nombre;
            institucion.Direccion = Input.Direccion;
            institucion.Dominio = Input.Dominio;
            institucion.Ruc = Input.Ruc;
            institucion.Telefono = Input.Telefono;
            institucion.CiudadId = Input.CiudadId;

            // Actualizar logo solo si se subió uno nuevo
            if (Input.Logo != null)
            {
                using var memoryStream = new MemoryStream();
                await Input.Logo.CopyToAsync(memoryStream);
                institucion.Logo = memoryStream.ToArray();
            }

            // Guardar los cambios
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "La institución fue editada correctamente.";
            return RedirectToPage("/Admin/VerInstitucionAdm");
        }

        public async Task<JsonResult> OnGetCiudadesPorProvincia(int provinciaId)
        {
            var ciudades = await _context.Ciudades
                .Where(c => c.ProvinciaId == provinciaId)
                .Select(c => new { id = c.Id, nombre = c.Nombre })
                .ToListAsync();

            return new JsonResult(ciudades);
        }

        private async Task CargarProvinciasYCiudades(int provinciaId)
        {
            Provincias = await _context.Provincias
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })
                .ToListAsync();

            Ciudades = await _context.Ciudades
                .Where(c => c.ProvinciaId == provinciaId)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToListAsync();
        }
    }
}
