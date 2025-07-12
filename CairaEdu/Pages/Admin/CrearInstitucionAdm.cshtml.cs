using CairaEdu.Data.Context;
using CairaEdu.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CrearInstitucionAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CrearInstitucionAdmModel(ApplicationDbContext context)
        {
            _context = context;
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
                CiudadId = Input.CiudadId,
                Estado = Input.Estado,
                Logo = logoBytes
            };

            _context.Instituciones.Add(institucion);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/VerInstitucionAdm");
        }

        public List<SelectListItem> Ciudades { get; set; } = new();

        public void OnGet()
        {
            Ciudades = _context.Ciudades
                .Where(c => c.Estado == 'A')
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToList();
        }

    }
}
