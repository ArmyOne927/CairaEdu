using CairaEdu.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Pages.Admin
{
    public class CrearInstitucionAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CrearInstitucionAdmModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Propiedades del formulario
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? LogoBase64 { get; set; }

        public class InputModel
        {
            public int inst_id { get; set; }
            public string inst_Nombre { get; set; }
            public string inst_direccion { get; set; }
            public string inst_ruc { get; set; }
            public string inst_telefono { get; set; }
            public string inst_dominio { get; set; }
            public string inst_ciudad { get; set; }
            public string inst_estado { get; set; }
        }

        public void OnGet(bool edit = false)
        {
            //if (edit)
            //{
            //    // buscar la institución relacionada con el usuario
            //    var institucion = _context.Instituciones.FirstOrDefault(); // Reemplaza con lógica real

            //    if (institucion != null)
            //    {
            //        Input = new InputModel
            //        {
            //            inst_id = institucion.inst_id,
            //            inst_Nombre = institucion.inst_Nombre,
            //            inst_direccion = institucion.inst_direccion,
            //            inst_ruc = institucion.inst_ruc,
            //            inst_telefono = institucion.inst_telefono,
            //            inst_dominio = institucion.inst_dominio,
            //            inst_ciudad = institucion.inst_ciudad,
            //            inst_estado = institucion.inst_estado
            //        };

            //        if (institucion.inst_Logo != null)
            //            LogoBase64 = Convert.ToBase64String(institucion.inst_Logo);
        }
    }
}


