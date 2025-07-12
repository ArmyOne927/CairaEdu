namespace CairaEdu.Data.Entities
{
    public class Provincia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public char Estado { get; set; }

        // Relación uno a muchos con Ciudad
        public ICollection<Ciudad> Ciudades { get; set; }
    }
}
