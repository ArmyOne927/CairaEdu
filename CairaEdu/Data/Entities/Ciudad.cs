namespace CairaEdu.Data.Entities
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public char Estado { get; set; }

        // FK hacia Provincia
        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }

        // Relación uno a muchos con Institucion
        public ICollection<Institucion> Instituciones { get; set; }
    }
}
