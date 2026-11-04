using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla Estudiantes: datos académicos del estudiante.
    // Las credenciales (correo y contraseña) se guardan en las tablas de Identity (AspNetUsers).
    [Index(nameof(Cedula), IsUnique = true)]
    [Index(nameof(UsuarioId), IsUnique = true)]
    public class Estudiante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty;

        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }

        // Enlace con el usuario de ASP.NET Core Identity.
        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        public IdentityUser? Usuario { get; set; }

        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
