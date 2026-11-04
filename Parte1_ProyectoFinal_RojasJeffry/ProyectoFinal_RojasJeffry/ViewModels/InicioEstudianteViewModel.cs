namespace ProyectoFinal_RojasJeffry.ViewModels
{
    // Datos de la página de inicio del estudiante (HU-03).
    public class InicioEstudianteViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public string Cuatrimestre { get; set; } = string.Empty;
        public int CursosMatriculados { get; set; }
        public int Creditos { get; set; }
        public int CursosDisponibles { get; set; }
    }
}
