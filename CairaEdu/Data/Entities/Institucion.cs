namespace CairaEdu.Data.Entities
{
    public class Institucion
    {
        public int Id { get; set; }
        public byte[] Logo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Dominio { get; set; }
        public string Ruc { get; set; }
        public string Telefono { get; set; }
        public char Estado { get; set; }

        // Fk hacia provincia
        public int ProvinciaId {  get; set; }
        public Provincia provincia { get; set; }    

        // FK hacia Ciudad
        public int CiudadId { get; set; }
        public Ciudad Ciudad { get; set; }
    }
}
