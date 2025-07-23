using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Core.ViewModels
{
    public class EditarParaleloViewModel
    {
        public int ParaleloId { get; set; }

        [Required(ErrorMessage = "El nombre del paralelo es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Estudiantes asignados")]
        public List<string> EstudiantesSeleccionados { get; set; } = new();

        public List<SelectListItem> ListaEstudiantes { get; set; } = new();
    }
}
