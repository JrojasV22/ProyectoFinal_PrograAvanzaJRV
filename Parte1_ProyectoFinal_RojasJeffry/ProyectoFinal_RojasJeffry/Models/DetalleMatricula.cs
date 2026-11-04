namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla DetallesMatricula: cursos incluidos en cada matrícula (detalle).
    public class DetalleMatricula
    {
        public int Id { get; set; }

        public int MatriculaId { get; set; }
        public Matricula? Matricula { get; set; }

        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
    }
}
