using CairaEdu.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    public class VerInstitucionAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public VerInstitucionAdmModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public string? LogoBase64 { get; set; }
        public string InstitucionNombre { get; set; } = "";
        public string InstitucionDireccion { get; set; } = "";
        public string InstitucionRUC { get; set; } = "";
        public string InstitucionTelefono { get; set; } = "";
        public string InstitucionCiudad { get; set; } = "";
        public string InstitucionEstado { get; set; } = "";
        public string InstitucionDominio { get; set; } = "";

        public bool UsuarioTieneInstitucion { get; set; }

        public void OnGet()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var institucion = _context.Users
                .Where(u => u.Id == usuarioId)
                .Include(u => u.Institucion)
                    .ThenInclude(i => i.Ciudad)
                .Select(u => u.Institucion)
                .FirstOrDefault();

            if (institucion != null)
            {
                UsuarioTieneInstitucion = true;

                InstitucionNombre = institucion.Nombre;
                InstitucionDireccion = institucion.Direccion;
                InstitucionRUC = institucion.Ruc;
                InstitucionTelefono = institucion.Telefono;
                InstitucionCiudad = institucion.Ciudad?.Nombre ?? "No definida";
                InstitucionEstado = institucion.Estado.ToString();
                InstitucionDominio = institucion.Dominio;

                if (institucion.Logo != null && institucion.Logo.Length > 0)
                {
                    LogoBase64 = Convert.ToBase64String(institucion.Logo);
                }
                else
                {
                    var defaultPath = Path.Combine(_env.WebRootPath, "img", "LogoDefaultInst.png");
                    LogoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(defaultPath));
                }
            }
            else
            {
                UsuarioTieneInstitucion = false;
                var defaultPath = Path.Combine(_env.WebRootPath, "img", "LogoDefaultInst.png");
                LogoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(defaultPath));
            }
        }
    }

}
