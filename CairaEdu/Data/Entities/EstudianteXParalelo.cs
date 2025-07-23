using CairaEdu.Data.Identity;

namespace CairaEdu.Data.Entities
{
    
    public class EstudianteXParalelo
    {
        public int Id { get; set; }

        public string EstudianteId { get; set; }
        public ApplicationUser Estudiante { get; set; }

        public int ParaleloId { get; set; }
        public Paralelo Paralelo { get; set; }
    }

}
