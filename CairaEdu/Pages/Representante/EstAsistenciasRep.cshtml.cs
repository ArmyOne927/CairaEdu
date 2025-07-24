using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Representante
{
    public class EstAsistenciasRepModel : PageModel
    {
        public string NombreEstudiante { get; set; } = "Ana María Torres";

        public List<EventoCalendarioVM> EventosCalendario { get; set; } = new();

        public void OnGet(int id)
        {
            // Simulación de estudiante obtenido por ID
            var estudiante = new
            {
                Nombre = "Ana María Torres",
                Foto = "/img/perfiles/ana.png"
            };

            ViewData["NombreEstudiante"] = estudiante.Nombre;
            ViewData["FotoEstudiante"] = estudiante.Foto;

            // Simular búsqueda de estudiante y asistencias
            EventosCalendario = new List<EventoCalendarioVM>
            {
                new() { title = "Matemáticas", start = "2025-07-19", docente = "Prof. Gómez", estado = "Asistencia" },
                new() { title = "Lengua", start = "2025-07-20", docente = "Prof. Silva", estado = "Retraso" },
                new() { title = "Ciencias", start = "2025-07-21", docente = "Prof. López", estado = "Falta" },
                new() { title = "Matemáticas", start = "2025-07-22", docente = "Prof. Gómez", estado = "Asistencia" }
            };
        }

        public class EventoCalendarioVM
        {
            public string title { get; set; }
            public string start { get; set; } // formato ISO date (ej. 2025-07-19)
            public string docente { get; set; }
            public string estado { get; set; }
        }
    }
}
