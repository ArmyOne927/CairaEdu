using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CairaEdu.Pages.Estudiante
{
    [Authorize(Roles = "Estudiante,Administrador")]
    public class MateriasEstModel : PageModel
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
            // Simulaci�n de estudiante obtenido por ID
            var estudiante = new
            {
                Nombre = "Ana Mar�a Torres",
                Foto = "/img/perfiles/ana.png"
            };

            ViewData["NombreEstudiante"] = estudiante.Nombre;
            ViewData["FotoEstudiante"] = estudiante.Foto;

            // Datos de ejemplo quemados
            MateriasAsignadas = new List<MateriaAsignada>
            {
                new MateriaAsignada
                {
                    Nombre = "Matem�ticas",
                    Competencias = "Razonamiento l�gico y resoluci�n de problemas.",
                    Objetivos = "Desarrollar habilidades de pensamiento matem�tico.",
                    Imagen = "/img/materias/5405a35b-477e-492f-8045-f2d7cdebc300.jpg"
                },
                new MateriaAsignada
                {
                    Nombre = "F�sica",
                    Competencias = "Comprensi�n de leyes f�sicas.",
                    Objetivos = "Analizar fen�menos naturales.",
                    Imagen = "/img/materias/13189074-2c67-47f3-9313-b7a7468a456b.jpg"
                },
                new MateriaAsignada
                {
                    Nombre = "Lengua y Literatura",
                    Competencias = "Comprensi�n lectora y redacci�n.",
                    Objetivos = "Fomentar el an�lisis cr�tico de textos.",
                    Imagen = "" // sin imagen, se mostrar� con texto
                }
            };
        }
    }
}
