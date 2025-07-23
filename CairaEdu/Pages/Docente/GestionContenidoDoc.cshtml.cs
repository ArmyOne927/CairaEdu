using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Docente
{
    [Authorize(Roles = "Docente,Administrador")]
    public class GestionContenidoDocModel : PageModel
    {
        [Authorize(Roles = "Docente,Administrador")]
        public class Material
        {
            public string Titulo { get; set; }
            public string Url { get; set; }
        }

        public class Tarea
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaEntrega { get; set; }
        }

        public class Evaluacion
        {
            public string Titulo { get; set; }
            public DateTime Fecha { get; set; }
            public string Instrucciones { get; set; }
        }

        public List<Material> Materiales { get; set; }
        public List<Tarea> Tareas { get; set; }
        public List<Evaluacion> Evaluaciones { get; set; }

        public void OnGet()
        {
            // Datos de ejemplo quemados
            Materiales = new List<Material>
            {
                new Material { Titulo = "Gu�a de F�sica", Url = "https://example.com/fisica.pdf" },
                new Material { Titulo = "Presentaci�n Matem�ticas", Url = "https://example.com/mate.pptx" }
            };

            Tareas = new List<Tarea>
            {
                new Tarea
                {
                    Titulo = "Tarea de Derivadas",
                    Descripcion = "Resolver los ejercicios de la p�gina 42 del libro.",
                    FechaEntrega = DateTime.Today.AddDays(5)
                }
            };

            Evaluaciones = new List<Evaluacion>
            {
                new Evaluacion
                {
                    Titulo = "Prueba de Gram�tica",
                    Fecha = DateTime.Today.AddDays(7),
                    Instrucciones = "Estudiar desde la unidad 1 hasta la unidad 3."
                }
            };
        }
    }
}
