using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Data.Entities
{
    [Index(nameof(Nombre), nameof(CursoId), IsUnique = true)]
    public class Paralelo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Nombre { get; set; }

        [Required]
        public int CursoId { get; set; }

        public Curso Curso { get; set; }
        public ICollection<HorarioParalelo> Horarios { get; set; } = new List<HorarioParalelo>();
        public ICollection<EstudianteXParalelo> Estudiantes { get; set; }
        public char Estado { get; set; } = 'A'; // Estado por defecto
    }

}
