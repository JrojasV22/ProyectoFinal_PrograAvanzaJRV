using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla Cursos: cada curso pertenece a una carrera y puede tener un docente asignado.
    [Index(nameof(Codigo), IsUnique = true)]
    public class Curso
    {
        // Tipos de curso permitidos 
        public static readonly string[] Tipos = { "Teórico", "Práctico", "Teórico-práctico" };

        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres.")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del curso es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Nombre del curso")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los créditos son obligatorios.")]
        [Range(1, 4, ErrorMessage = "Los créditos deben estar entre 1 y 4.")]
        [Display(Name = "Créditos")]
        public int Creditos { get; set; }

        [Required(ErrorMessage = "El tipo de curso es obligatorio.")]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El horario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El horario no puede tener más de 50 caracteres.")]
        public string Horario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cupo es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El cupo debe ser un número entre 1 y 100.")]
        [Display(Name = "Cupo máximo")]
        public int Cupo { get; set; }

        [Required(ErrorMessage = "Seleccione una carrera.")]
        [Display(Name = "Carrera")]
        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }

        
        [Display(Name = "Docente")]
        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public ICollection<DetalleMatricula> DetallesMatricula { get; set; } = new List<DetalleMatricula>();
    }
}
