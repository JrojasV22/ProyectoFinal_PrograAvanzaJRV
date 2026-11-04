using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_RojasJeffry.ViewModels
{
    // Datos del formulario de registro de estudiantes (HU-01 y HU-02).
    // Las validaciones se aplican en el servidor y, con jQuery Validation, también en el navegador.
    public class RegistroViewModel
    {
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
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar")]
        public string ConfirmarContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione una carrera.")]
        [Display(Name = "Carrera")]
        public int? CarreraId { get; set; }
    }
}
