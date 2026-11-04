using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    // Panel de administración (HU-09). Solo para el rol Administrador.
    [Authorize(Roles = Roles.Administrador)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = new PanelViewModel
            {
                TotalCarreras = await _db.Carreras.CountAsync(),
                TotalCursos = await _db.Cursos.CountAsync(),
                TotalDocentes = await _db.Docentes.CountAsync(),
                TotalEstudiantes = await _db.Estudiantes.CountAsync(),
                Ocupacion = await _db.Cursos
                    .OrderByDescending(c => c.DetallesMatricula.Count)
                    .ThenBy(c => c.Nombre)
                    .Take(5)
                    .Select(c => new OcupacionCurso
                    {
                        Curso = c.Nombre,
                        Carrera = c.Carrera!.Codigo,
                        Docente = c.Docente != null ? c.Docente.NombreCompleto : "Sin asignar",
                        Matriculados = c.DetallesMatricula.Count,
                        Cupo = c.Cupo
                    })
                    .ToListAsync()
            };

            return View(modelo);
        }
    }
}
