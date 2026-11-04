using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla Matriculas: transacción de matrícula de un estudiante en un cuatrimestre (encabezado).
    public class Matricula
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }

        [Required]
        [StringLength(30)]
        public string Cuatrimestre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Total de créditos")]
        public int TotalCreditos { get; set; }

        public ICollection<DetalleMatricula> Detalles { get; set; } = new List<DetalleMatricula>();
    }
}
