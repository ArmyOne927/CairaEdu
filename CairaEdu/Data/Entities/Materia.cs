using CairaEdu.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CairaEdu.Data.Entities
{
    public class Materia
    {
        [Key]
        [Column("mat_id")]
        public int Id { get; set; }

        [Column("mat_nombre")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Column("mat_competencias")]
        [MaxLength(1000)]
        public string? Competencias { get; set; }

        [Column("mat_objetivos")]
        [MaxLength(1000)]
        public string? Objetivos { get; set; }

        [Column("mat_imagen")]
        [MaxLength(64)]
        public string? Imagen { get; set; }

        [Column("mat_estado")]
        [MaxLength(1)]
        public string Estado { get; set; }

        public ICollection<MateriaProfesor> MateriaProfesores { get; set; }

        [Column("mat_inst_id")]
        public int InstitucionId { get; set; }

        [ForeignKey("InstitucionId")]
        public Institucion Institucion { get; set; }

    }
}
