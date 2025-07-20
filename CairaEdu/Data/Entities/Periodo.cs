using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CairaEdu.Data.Entities
{
    public class Periodo
    {
        public int Id { get; set; }

        [Required]
        public int CicloLectivoId { get; set; }
        [ForeignKey("CicloLectivoId")]
        public CicloLectivo CicloLectivo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [StringLength(30)]
        public string Tipo { get; set; }

        public bool EsActivo { get; set; } = true;
    }
}
