using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_RojasJeffry.Models
{
    // Tabla DocentesCarreras: relación muchos a muchos entre docentes y carreras.
    [PrimaryKey(nameof(DocenteId), nameof(CarreraId))]
    public class DocenteCarrera
    {
        public int DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }
    }
}
