using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    public class CrearUsuarioAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CrearUsuarioAdmModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();
        public List<SelectListItem> Instituciones { get; set; }
        public List<SelectListItem> TiposDocumento { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public class InputModel
        {
            [Required]
            public string? Nombres { get; set; }

            [Required]
            public string? Apellidos { get; set; }

            [Required]
            public string Documento { get; set; }

            [Required]
            public int TipoDocumentoId { get; set; }

            [Required]
            public string Role {  get; set; }
            [Phone]
            public string? Telefono { get; set; }

            public DateTime? FechaNacimiento { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public char Estado { get; set; }  // A, I, etc.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            //id del usuario autenticado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Obtener la institución del usuario autenticado
            var institucion = await _context.Users
                .Where(u => u.Id == usuarioId)
                .Include(u => u.Institucion)
                .Select(u => u.Institucion)
                .FirstOrDefaultAsync();

            if (institucion == null)
            {
                ModelState.AddModelError(string.Empty, "No se pudo obtener la institución del usuario actual.");
                return Page();
            }

            //creacion del usuario con la institucion del logueado
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Nombres = Input.Nombres,
                Apellidos = Input.Apellidos,
                TipoDocumentoId = Input.TipoDocumentoId,
                Documento = Input.Documento,
                PhoneNumber = Input.Telefono,
                Estado = Input.Estado,
                InstitucionId = institucion.Id,
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Asignar rol
                await _userManager.AddToRoleAsync(user, Input.Role);
                return RedirectToPage("VerUsuarioAdm");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
