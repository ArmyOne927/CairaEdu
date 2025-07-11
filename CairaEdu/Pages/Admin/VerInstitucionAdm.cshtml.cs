using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Pages.Admin
{
    public class VerInstitucionAdmModel : PageModel
    {
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
            // Lógica para verificar si el usuario tiene institución

            var usuarioId = User.Identity?.Name;

            UsuarioTieneInstitucion = false; //ESTO PROVISIONALMENTE TIENE FALSE, PORQUE NO HE MANDADO LA BASE DE DATOS //_context.Instituciones.Any(i => i.UsuarioId == usuarioId);

            // Aquí deberías obtener los datos desde la base de datos
            // ESTE EJEMPLO ES CON LOS DATOS QUEMADOS 
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
