namespace CairaEdu.Data.Entities
{
    public class HorarioParalelo
    {
        public int Id { get; set; }
        public int ParaleloId { get; set; }
        public int MateriaId { get; set; }

        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }

        public int? MateriaProfesorId { get; set; }
        public string? Aula { get; set; } = null;
        public char Estado { get; set; } = 'A'; // A = Activo, I = Inactivo (borrado lógico)

        public virtual Paralelo Paralelo { get; set; }
        public virtual Materia Materia { get; set; }
        public virtual MateriaProfesor? MateriaProfesor { get; set; }

    }

}
