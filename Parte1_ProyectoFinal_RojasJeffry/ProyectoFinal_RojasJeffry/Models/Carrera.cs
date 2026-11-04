using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla Carreras: carreras universitarias que ofrece la institución.
    [Index(nameof(Codigo), IsUnique = true)]
    public class Carrera
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres.")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La descripción no puede tener más de 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
        public ICollection<DocenteCarrera> DocentesCarreras { get; set; } = new List<DocenteCarrera>();
    }
}
