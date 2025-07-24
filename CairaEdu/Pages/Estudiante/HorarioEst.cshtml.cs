using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Pages.Estudiante
{
    public class HorarioEstModel : PageModel
    {
        public string NombreEstudiante { get; set; }
        public List<EventoHorarioVM> EventosHorario { get; set; }

        public class EventoHorarioVM
        {
            [Required]
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

        public IActionResult OnGet(int id)
        {
            EventosHorario = new List<EventoHorarioVM>
            {
                new EventoHorarioVM {
                    title = "Matemáticas",
                    start = "2025-07-22T08:00:00",
                    end = "2025-07-22T09:00:00"
                },
                new EventoHorarioVM {
                    title = "Lengua",
                    start = "2025-07-22T09:00:00",
                    end = "2025-07-22T10:00:00"
                },
                new EventoHorarioVM {
                    title = "Ciencias Naturales",
                    start = "2025-07-24T10:00:00",
                    end = "2025-07-24T11:30:00"
                },
                new EventoHorarioVM {
                    title = "Educación Física",
                    start = "2025-07-25T11:00:00",
                    end = "2025-07-25T12:00:00"
                }
            };

            return Page();
        }
    }
}
