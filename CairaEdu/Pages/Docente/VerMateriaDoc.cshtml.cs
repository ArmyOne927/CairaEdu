using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Docente
{
    [Authorize(Roles = "Docente,Administrador")]
    public class VerMateriaDocModel : PageModel
    {
        public class MateriaAsignada
        {
            public string Nombre { get; set; }
            public string Competencias { get; set; }
            public string Objetivos { get; set; }
            public string CursoNombre { get; set; }
            public string ParaleloNombre { get; set; }
            public string Imagen { get; set; }
        }

        public List<MateriaAsignada> MateriasAsignadas { get; set; }

        public void OnGet()
        {
            // Datos de ejemplo quemados
            MateriasAsignadas = new List<MateriaAsignada>
            {
                new MateriaAsignada
                {
                    Nombre = "Matemáticas",
                    Competencias = "Razonamiento lógico y resolución de problemas.",
                    Objetivos = "Desarrollar habilidades de pensamiento matemático.",
                    CursoNombre = "1° Bachillerato",
                    ParaleloNombre = "A",
                    Imagen = "/img/materias/5405a35b-477e-492f-8045-f2d7cdebc300.jpg"
                },
                new MateriaAsignada
                {
                    Nombre = "Física",
                    Competencias = "Comprensión de leyes físicas.",
                    Objetivos = "Analizar fenómenos naturales.",
                    CursoNombre = "2° Bachillerato",
                    ParaleloNombre = "B",
                    Imagen = "/img/materias/13189074-2c67-47f3-9313-b7a7468a456b.jpg"
                },
                new MateriaAsignada
                {
                    Nombre = "Lengua y Literatura",
                    Competencias = "Comprensión lectora y redacción.",
                    Objetivos = "Fomentar el análisis crítico de textos.",
                    CursoNombre = "1° Bachillerato",
                    ParaleloNombre = "C",
                    Imagen = "" // sin imagen, se mostrará con texto
                }
            };
        }
    }
}
