using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Representante
{
    public class VerEstudiantesRepModel : PageModel
    {
        public List<EstudianteVM> Estudiantes { get; set; } = new List<EstudianteVM>
        {
            new EstudianteVM {
                Id = 1,
                NombreCompleto = "Ana María Torres",
                Curso = "Superior 8° (EGB)",
                Paralelo = "A",
                FotoPerfilUrl = "/img/perfiles/ana.png"
            },
            new EstudianteVM {
                Id = 2,
                NombreCompleto = "Luis Fernando Pérez",
                Curso = "Superior 8° (EGB)",
                Paralelo = "B",
                FotoPerfilUrl = "/img/perfiles/luis.png"
            },
            new EstudianteVM {
                Id = 3,
                NombreCompleto = "Andrés Gómez",
                Curso = "Superior 9° (EGB)",
                Paralelo = "C",
                FotoPerfilUrl = "/img/perfiles/Gomez.png"
            }
        };

        public class EstudianteVM
        {
            public int Id { get; set; }
            public string NombreCompleto { get; set; }
            public string Curso { get; set; }
            public string Paralelo { get; set; }
            public string FotoPerfilUrl { get; set; }
        }

        public void OnGet()
        {
        }
    }
}
