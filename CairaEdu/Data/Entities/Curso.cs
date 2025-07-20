using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CairaEdu.Data.Entities
{
    [Index(nameof(Nombre), nameof(CicloLectivoId), IsUnique = true)]
    public class Curso
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int CicloLectivoId { get; set; }

        public CicloLectivo CicloLectivo { get; set; }

        public ICollection<Paralelo> Paralelos { get; set; } = new List<Paralelo>();
    }

}
