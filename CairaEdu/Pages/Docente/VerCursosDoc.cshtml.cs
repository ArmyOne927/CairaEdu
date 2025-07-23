using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Docente
{
    [Authorize(Roles = "Docente,Administrador")]
    public class VerCursosDocModel : PageModel
    {
        public class Estudiante
        {
            public string Nombre { get; set; }
            public string Cedula { get; set; }
        }

        public class CursoAsignado
        {
            public int Id { get; set; }
            public string NombreCurso { get; set; }
            public string Paralelo { get; set; }
            public List<Estudiante> Estudiantes { get; set; }
        }

        public List<CursoAsignado> Cursos { get; set; }

        public void OnGet()
        {
            Cursos = new List<CursoAsignado>
            {
                new CursoAsignado
                {
                    Id = 1,
                    NombreCurso = "1° Bachillerato",
                    Paralelo = "A",
                    Estudiantes = new List<Estudiante>
                    {
                        new Estudiante { Nombre = "Juan Pérez", Cedula = "1101122334" },
                        new Estudiante { Nombre = "Ana Torres", Cedula = "1101456789" }
                    }
                },
                new CursoAsignado
                {
                    Id = 2,
                    NombreCurso = "2° Bachillerato",
                    Paralelo = "B",
                    Estudiantes = new List<Estudiante>
                    {
                        new Estudiante { Nombre = "Carlos Ruiz", Cedula = "1101987654" },
                        new Estudiante { Nombre = "María Gómez", Cedula = "1101678432" }
                    }
                }
            };
        }
    }
}
