using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class MateriaProfesor
{
    [Key]
    [Column("mp_id")]
    public int Id { get; set; }

    [Column("mp_user_id")]
    public string UserId { get; set; }

    [Column("mp_mat_id")]
    public int MateriaId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser Profesor { get; set; }

    [ForeignKey("MateriaId")]
    public Materia Materia { get; set; }
}
