using System.ComponentModel.DataAnnotations;
using CairaEdu.Data.Context;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CairaEdu.Pages.Admin
{
    public class EditarUsuarioAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public EditarUsuarioAdmModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }


        [BindProperty]
        public InputModel Input { get; set; } = new();
        public List<SelectListItem> Instituciones { get; set; }
        public List<SelectListItem> TiposDocumento { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public class InputModel {
            public string Id { get; set; }  
            public byte[]? Foto { get; set; }
            public IFormFile? FotoSubida { get; set; } //para subir una nueva
            public bool EliminarFoto { get; set; } //para eliminar la fot
            public string? Nombres { get; set; }
            public string? Apellidos { get; set; }
            public string Documento { get; set; }
            public int TipoDocumentoId { get; set; }
            public string Role { get; set; }
            [Phone]
            public string? Telefono { get; set; }

            public string Email { get; set; }
            public char Estado { get; set; }  // A, I, etc.
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Console.WriteLine("ID recibido en EditarUsuarioAdm: " + id);

            var actual = await _userManager.GetUserAsync(User);
            if (actual?.InstitucionId == null)
            {
                TempData["Error"] = "No tiene institución asignada.";
                return RedirectToPage("/Admin/VerInstitucionAdm");
            }

            // Buscar al usuario que vamos a editar
            var usuario = await _context.Users
                .Include(u => u.TipoDocumento)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Precargar datos en InputModel
            Input = new InputModel
            {
                Id = usuario.Id,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Documento = usuario.Documento,
                TipoDocumentoId = usuario.TipoDocumentoId,
                Telefono = usuario.PhoneNumber,
                Email = usuario.Email,
                Estado = usuario.Estado,
                Foto = usuario.Foto,
                Role = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault() ?? ""
            };

            // Cargar combos
            TiposDocumento = await _context.TipoDocumentos
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Descripcion
                }).ToListAsync();

            Roles = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            Instituciones = await _context.Instituciones
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre
                }).ToListAsync();
            Console.WriteLine("prueba de get");
            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    Console.WriteLine(">>> MÉTODO POST EJECUTADO <<<");
        //    return RedirectToPage("/Admin/VerUsuarioAdm");
        //}


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errores)
                    Console.WriteLine("Error de validación: " + error);

                await CargarCombosAsync();
                return Page();
            }


            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == Input.Id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToPage("/Admin/VerUsuarioAdm");
            }

            // Actualizar datos
            usuario.Nombres = Input.Nombres;
            usuario.Apellidos = Input.Apellidos;
            usuario.Documento = Input.Documento;
            usuario.TipoDocumentoId = Input.TipoDocumentoId;
            usuario.PhoneNumber = Input.Telefono;
            usuario.Email = Input.Email;
            usuario.UserName = Input.Email;
            usuario.Estado = Input.Estado;

            // Procesar foto
            if (Input.EliminarFoto)
            {
                usuario.Foto = null;
            }
            else if (Input.FotoSubida != null && Input.FotoSubida.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await Input.FotoSubida.CopyToAsync(memoryStream);
                usuario.Foto = memoryStream.ToArray();
            }

            // Guardar cambios
            _context.Entry(usuario).State = EntityState.Modified;
            var cambios = await _context.SaveChangesAsync();
            Console.WriteLine($"Cambios guardados forzadamente: {cambios}");


            // Actualizar rol
            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);
            await _userManager.AddToRoleAsync(usuario, Input.Role);

            TempData["Success"] = "Usuario actualizado correctamente.";
            Input.Foto = usuario.Foto;
            await CargarCombosAsync();
            Console.WriteLine("POST ejecutado**************************************");
            return RedirectToPage("/Admin/VerUsuarioAdm");
        }

        private async Task CargarCombosAsync()
        {
            TiposDocumento = await _context.TipoDocumentos
                .Where(t => t.Estado == 'A')
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Descripcion
                }).ToListAsync();

            Roles = _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();

            Instituciones = await _context.Instituciones
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre
                }).ToListAsync();
        }


    }
}
