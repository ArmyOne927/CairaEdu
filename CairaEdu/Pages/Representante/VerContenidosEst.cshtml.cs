using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Representante
{
    public class VerContenidosEstModel : PageModel
    {
        public string NombreEstudiante { get; set; } = "Ana María Torres";

        public List<MaterialVM> Materiales { get; set; } = new();
        public List<TareaVM> Tareas { get; set; } = new();
        public List<EvaluacionVM> Evaluaciones { get; set; } = new();

        public void OnGet(int id)
        {
            // Datos simulados
            Materiales = new List<MaterialVM>
            {
                new() { Titulo = "Guía de Ciencias", Descripcion = "Material PDF sobre células", Url = "/docs/guia_ciencias.pdf" },
                new() { Titulo = "Video: Sistema Solar", Descripcion = "Enlace de YouTube", Url = "https://youtube.com/watch?v=solarvideo" }
            };

            Tareas = new List<TareaVM>
            {
                new() { Titulo = "Ensayo sobre la célula", Descripcion = "Mínimo 300 palabras", FechaEntrega = DateTime.Today.AddDays(2), Entregada = true },
                new() { Titulo = "Mapa conceptual del sistema digestivo", Descripcion = "Realizar en hoja A4", FechaEntrega = DateTime.Today.AddDays(5), Entregada = false }
            };

            Evaluaciones = new List<EvaluacionVM>
            {
                new() { Titulo = "Prueba de Ciencias", Fecha = DateTime.Today.AddDays(-1), Instrucciones = "50 preguntas de opción múltiple", Estado = "Calificada" },
                new() { Titulo = "Examen de Lengua", Fecha = DateTime.Today.AddDays(1), Instrucciones = "Texto argumentativo", Estado = "Pendiente" }
            };
        }

        public class MaterialVM
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string Url { get; set; }
        }

        public class TareaVM
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaEntrega { get; set; }
            public bool Entregada { get; set; }
        }

        public class EvaluacionVM
        {
            public string Titulo { get; set; }
            public DateTime Fecha { get; set; }
            public string Instrucciones { get; set; }
            public string Estado { get; set; } // "Pendiente", "Realizada", "Calificada"
        }
    }
}
