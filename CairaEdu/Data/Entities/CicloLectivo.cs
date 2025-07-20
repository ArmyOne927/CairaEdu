using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CairaEdu.Data.Entities
{
    public class CicloLectivo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Region { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required]
        public int InstitucionId { get; set; }
        [ForeignKey("InstitucionId")]
        public Institucion Institucion { get; set; }
        public ICollection<Periodo> Periodos { get; set; }
        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}