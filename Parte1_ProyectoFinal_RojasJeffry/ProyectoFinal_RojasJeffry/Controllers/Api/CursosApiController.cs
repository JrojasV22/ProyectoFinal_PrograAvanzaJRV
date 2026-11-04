using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;

namespace ProyectoFinal_RojasJeffry.Controllers.Api
{
    // Endpoint JSON que se consume con AJAX para cargar los cursos de una carrera
    // sin recargar la página (formulario de registro, HU-02).
    [ApiController]
    [Route("api/cursos")]
    [AllowAnonymous]
    public class CursosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CursosApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /api/cursos?carreraId=1
        [HttpGet]
        public async Task<IActionResult> PorCarrera([FromQuery] int carreraId)
        {
            var cursos = await _db.Cursos
                .Where(c => c.CarreraId == carreraId)
                .OrderBy(c => c.Codigo)
                .Select(c => new
                {
                    c.Id,
                    c.Codigo,
                    c.Nombre,
                    c.Creditos,
                    c.Tipo,
                    c.Horario,
                    Docente = c.Docente != null ? c.Docente.NombreCompleto : "Sin asignar"
                })
                .ToListAsync();

            return Ok(cursos);
        }
    }
}
