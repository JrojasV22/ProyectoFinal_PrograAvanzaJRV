using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    // Pantallas del estudiante. Solo pueden entrar usuarios con el rol Estudiante.
    [Authorize(Roles = Roles.Estudiante)]
    public class EstudianteController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public EstudianteController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Interfaz principal del estudiante (HU-03).
        public async Task<IActionResult> Inicio()
        {
            var usuarioId = _userManager.GetUserId(User);
            var estudiante = await _db.Estudiantes
                .Include(e => e.Carrera)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

            if (estudiante == null)
            {
                return RedirectToAction("AccesoDenegado", "Cuenta");
            }

            var cursosMatriculados = await _db.DetallesMatricula
                .Where(d => d.Matricula!.EstudianteId == estudiante.Id)
                .Select(d => d.Curso!.Creditos)
                .ToListAsync();

            var modelo = new InicioEstudianteViewModel
            {
                Nombre = estudiante.NombreCompleto,
                Carrera = estudiante.Carrera?.Nombre ?? string.Empty,
                Cuatrimestre = Cuatrimestre.Actual(),
                CursosMatriculados = cursosMatriculados.Count,
                Creditos = cursosMatriculados.Sum(),
                CursosDisponibles = await _db.Cursos.CountAsync(c => c.CarreraId == estudiante.CarreraId)
            };

            return View(modelo);
        }
    }
}
