using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Representante
{
    public class EstAsistenciasRepModel : PageModel
    {
        public string NombreEstudiante { get; set; } = "Ana Mar�a Torres";

        public List<EventoCalendarioVM> EventosCalendario { get; set; } = new();

        public void OnGet(int id)
        {
            // Simulaci�n de estudiante obtenido por ID
            var estudiante = new
            {
                Nombre = "Ana Mar�a Torres",
                Foto = "/img/perfiles/ana.png"
            };

            ViewData["NombreEstudiante"] = estudiante.Nombre;
            ViewData["FotoEstudiante"] = estudiante.Foto;

            // Simular b�squeda de estudiante y asistencias
            EventosCalendario = new List<EventoCalendarioVM>
            {
                new() { title = "Matem�ticas", start = "2025-07-19", docente = "Prof. G�mez", estado = "Asistencia" },
                new() { title = "Lengua", start = "2025-07-20", docente = "Prof. Silva", estado = "Retraso" },
                new() { title = "Ciencias", start = "2025-07-21", docente = "Prof. L�pez", estado = "Falta" },
                new() { title = "Matem�ticas", start = "2025-07-22", docente = "Prof. G�mez", estado = "Asistencia" }
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
