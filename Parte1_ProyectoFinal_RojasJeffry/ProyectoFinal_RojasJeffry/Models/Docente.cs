using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla Docentes: profesores que se pueden asignar a cursos y carreras.
    [Index(nameof(Cedula), IsUnique = true)]
    [Index(nameof(Correo), IsUnique = true)]
    public class Docente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20, ErrorMessage = "La cédula no puede tener más de 20 caracteres.")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(100, ErrorMessage = "La especialidad no puede tener más de 100 caracteres.")]
        public string Especialidad { get; set; } = string.Empty;

        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public ICollection<DocenteCarrera> DocentesCarreras { get; set; } = new List<DocenteCarrera>();
    }
}
