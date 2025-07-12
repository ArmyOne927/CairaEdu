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
            public string Telefono { get; set; }
            public int ProvinciaId { get; set; }
            public int CiudadId { get; set; }
            public char Estado { get; set; }
            public IFormFile? Logo { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errores)
                    Console.WriteLine(error);
                return Page();
            }


            byte[]? logoBytes = null;
            if (Input.Logo != null)
            {
                using var memoryStream = new MemoryStream();
                await Input.Logo.CopyToAsync(memoryStream);
                logoBytes = memoryStream.ToArray();
            }

            var institucion = new Institucion
            {
                Nombre = Input.Nombre,
                Direccion = Input.Direccion,
                Dominio = Input.Dominio,
                Ruc = Input.Ruc,
                Telefono = Input.Telefono,
                ProvinciaId= Input.ProvinciaId,
                CiudadId = Input.CiudadId,
                Estado = Input.Estado,
                Logo = logoBytes
            };

            _context.Instituciones.Add(institucion);
            await _context.SaveChangesAsync();

            //Obtener usuario autenticado
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
                .Where(p => p.Estado== 'A')
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text= p.Nombre
                })
                .ToList();
        }

        public List<SelectListItem> Ciudades { get; set; } = new();
        // using Microsoft.AspNetCore.Mvc;
        public JsonResult OnGetCiudadesPorProvincia(int ProvinciaId)
        {
            var ciudades = _context.Ciudades
                .Where(c => c.ProvinciaId == ProvinciaId)
                .Select(c => new { c.Id, c.Nombre })
                .ToList();

            return new JsonResult(ciudades);
        }


    }
}
