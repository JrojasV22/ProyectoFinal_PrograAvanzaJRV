namespace ProyectoFinal_RojasJeffry.ViewModels
{
    // Datos del panel de administración (HU-09).
    public class PanelViewModel
    {
        public int TotalCarreras { get; set; }
        public int TotalCursos { get; set; }
        public int TotalDocentes { get; set; }
        public int TotalEstudiantes { get; set; }
        public List<OcupacionCurso> Ocupacion { get; set; } = new();
    }

    public class OcupacionCurso
    {
        public string Curso { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public string Docente { get; set; } = string.Empty;
        public int Matriculados { get; set; }
        public int Cupo { get; set; }
    }
}
