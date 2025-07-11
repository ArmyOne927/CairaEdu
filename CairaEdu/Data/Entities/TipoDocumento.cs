using CairaEdu.Data.Identity;

namespace CairaEdu.Data.Entities
{
    public class TipoDocumento
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public char Estado { get; set; }

        // Relación uno a muchos con ApplicationUser
        public ICollection<ApplicationUser> Usuarios { get; set; }
    }
}
