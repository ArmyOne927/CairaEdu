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

        public VerInstitucionAdmModel(ApplicationDbContext context)
        {
            _context = context;
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
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Usa el ID del usuario autenticado
            var Institucion = _context.Users
                .Where(u => u.Id == usuarioId)
                .Select(u => u.InstitucionId)
                .FirstOrDefault();

            // Verifica si hay una institución asociada al usuario actual
            UsuarioTieneInstitucion = _context.Instituciones.Any(i => i.Id == Institucion);

            // EJEMPLO QUEMADO (debes reemplazarlo con datos reales si la institución existe)
            InstitucionNombre = "Colegio Caira";
            InstitucionDireccion = "Av. Siempre Viva 123";
            InstitucionRUC = "1799999999001";
            InstitucionTelefono = "0999999999";
            InstitucionCiudad = "Quito";
            InstitucionEstado = "A";
            InstitucionDominio = "colegiocaira";

            byte[] logoBytes = System.IO.File.ReadAllBytes("wwwroot/img/logoconovalo.png"); // o desde la BD
            LogoBase64 = Convert.ToBase64String(logoBytes);
        }
    }
}
