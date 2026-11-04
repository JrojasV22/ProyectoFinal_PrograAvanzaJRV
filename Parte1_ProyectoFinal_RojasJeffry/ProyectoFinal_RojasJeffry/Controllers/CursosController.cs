using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.Models;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    // Gestión de cursos y su asociación con carreras (HU-11).
    [Authorize(Roles = Roles.Administrador)]
    public class CursosController : Controller
    {
        private const int TamanoPagina = 10;
        private readonly ApplicationDbContext _db;

        public CursosController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Cursos  (la tabla se carga con AJAX desde la acción Tabla)
        public async Task<IActionResult> Index(int? carreraId)
        {
            await CargarCarrerasAsync(carreraId);
            ViewData["CarreraId"] = carreraId;
            return View();
        }

        // GET: /Cursos/Tabla?carreraId=1&pagina=1
        // Devuelve solo la tabla (vista parcial) para actualizarla por AJAX al cambiar el filtro.
        public async Task<IActionResult> Tabla(int? carreraId, int pagina = 1)
        {
            var consulta = _db.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .AsNoTracking();

            if (carreraId.HasValue)
            {
                consulta = consulta.Where(c => c.CarreraId == carreraId.Value);
            }

            var lista = await ListaPaginada<Curso>.CrearAsync(consulta.OrderBy(c => c.Codigo), pagina, TamanoPagina);
            return PartialView("_Tabla", lista);
        }

        // GET: /Cursos/Create
        public async Task<IActionResult> Create(int? carreraId)
        {
            await CargarListasAsync(carreraId);
            return View(new Curso { CarreraId = carreraId ?? 0, Creditos = 3, Cupo = 25 });
        }

        // POST: /Cursos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Creditos,Tipo,Horario,Cupo,CarreraId")] Curso curso)
        {
            await ValidarAsync(curso);

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(curso.CarreraId);
                return View(curso);
            }

            _db.Cursos.Add(curso);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"El curso {curso.Nombre} se creó correctamente.";
            return RedirectToAction(nameof(Index), new { carreraId = curso.CarreraId });
        }

        // GET: /Cursos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var curso = await _db.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            await CargarListasAsync(curso.CarreraId);
            return View(curso);
        }

        // POST: /Cursos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Creditos,Tipo,Horario,Cupo,CarreraId")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            await ValidarAsync(curso);

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(curso.CarreraId);
                return View(curso);
            }

            var existente = await _db.Cursos.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            // El docente asignado no se cambia aquí (se asigna en su propia pantalla, HU-13).
            existente.Codigo = curso.Codigo;
            existente.Nombre = curso.Nombre;
            existente.Creditos = curso.Creditos;
            existente.Tipo = curso.Tipo;
            existente.Horario = curso.Horario;
            existente.Cupo = curso.Cupo;
            existente.CarreraId = curso.CarreraId;
            await _db.SaveChangesAsync();

            TempData["Exito"] = $"El curso {curso.Nombre} se actualizó correctamente.";
            return RedirectToAction(nameof(Index), new { carreraId = curso.CarreraId });
        }

        // GET: /Cursos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var curso = await _db.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Docente)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: /Cursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _db.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            if (await _db.DetallesMatricula.AnyAsync(d => d.CursoId == id))
            {
                TempData["Error"] = $"No se puede eliminar el curso {curso.Nombre} porque tiene estudiantes matriculados.";
                return RedirectToAction(nameof(Index));
            }

            _db.Cursos.Remove(curso);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"El curso {curso.Nombre} se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // Validaciones del servidor que no se pueden hacer con atributos.
        private async Task ValidarAsync(Curso curso)
        {
            if (await _db.Cursos.AnyAsync(c => c.Codigo == curso.Codigo && c.Id != curso.Id))
            {
                ModelState.AddModelError(nameof(curso.Codigo), "Ya existe un curso con ese código.");
            }

            if (!Curso.Tipos.Contains(curso.Tipo))
            {
                ModelState.AddModelError(nameof(curso.Tipo), "Seleccione un tipo de curso válido.");
            }

            if (!await _db.Carreras.AnyAsync(c => c.Id == curso.CarreraId))
            {
                ModelState.AddModelError(nameof(curso.CarreraId), "Seleccione una carrera válida.");
            }
        }

        private async Task CargarCarrerasAsync(int? seleccionada)
        {
            var carreras = await _db.Carreras.OrderBy(c => c.Nombre).AsNoTracking().ToListAsync();
            ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre", seleccionada);
        }

        private async Task CargarListasAsync(int? carreraSeleccionada)
        {
            await CargarCarrerasAsync(carreraSeleccionada);
            ViewBag.Tipos = new SelectList(Curso.Tipos);
        }
    }
}
