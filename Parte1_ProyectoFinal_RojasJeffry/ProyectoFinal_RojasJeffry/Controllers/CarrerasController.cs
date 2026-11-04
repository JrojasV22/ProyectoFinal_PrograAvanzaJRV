using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.Models;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    // Gestión de carreras: crear, listar, editar y eliminar (HU-10).
    [Authorize(Roles = Roles.Administrador)]
    public class CarrerasController : Controller
    {
        private const int TamanoPagina = 5;
        private readonly ApplicationDbContext _db;

        public CarrerasController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Carreras?buscar=ing&pagina=1  (lista con buscador y paginación)
        public async Task<IActionResult> Index(string? buscar, int pagina = 1)
        {
            var consulta = _db.Carreras.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(c => c.Nombre.Contains(buscar) || c.Codigo.Contains(buscar));
            }

            ViewData["Buscar"] = buscar;
            var lista = await ListaPaginada<Carrera>.CrearAsync(consulta.OrderBy(c => c.Nombre), pagina, TamanoPagina);
            return View(lista);
        }

        // GET: /Carreras/Create
        public IActionResult Create()
        {
            return View(new Carrera());
        }

        // POST: /Carreras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion")] Carrera carrera)
        {
            if (await _db.Carreras.AnyAsync(c => c.Codigo == carrera.Codigo))
            {
                ModelState.AddModelError(nameof(carrera.Codigo), "Ya existe una carrera con ese código.");
            }

            if (!ModelState.IsValid)
            {
                return View(carrera);
            }

            _db.Carreras.Add(carrera);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"La carrera {carrera.Nombre} se creó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Carreras/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var carrera = await _db.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }
            return View(carrera);
        }

        // POST: /Carreras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion")] Carrera carrera)
        {
            if (id != carrera.Id)
            {
                return NotFound();
            }

            if (await _db.Carreras.AnyAsync(c => c.Codigo == carrera.Codigo && c.Id != carrera.Id))
            {
                ModelState.AddModelError(nameof(carrera.Codigo), "Ya existe otra carrera con ese código.");
            }

            if (!ModelState.IsValid)
            {
                return View(carrera);
            }

            var existente = await _db.Carreras.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            existente.Codigo = carrera.Codigo;
            existente.Nombre = carrera.Nombre;
            existente.Descripcion = carrera.Descripcion;
            await _db.SaveChangesAsync();

            TempData["Exito"] = $"La carrera {carrera.Nombre} se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Carreras/Delete/5  (pantalla de confirmación)
        public async Task<IActionResult> Delete(int id)
        {
            var carrera = await _db.Carreras.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            ViewBag.TotalCursos = await _db.Cursos.CountAsync(c => c.CarreraId == id);
            ViewBag.TotalEstudiantes = await _db.Estudiantes.CountAsync(e => e.CarreraId == id);
            return View(carrera);
        }

        // POST: /Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrera = await _db.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }

            // No se puede eliminar una carrera con cursos, estudiantes o matrículas asociadas.
            var tieneRelaciones = await _db.Cursos.AnyAsync(c => c.CarreraId == id)
                || await _db.Estudiantes.AnyAsync(e => e.CarreraId == id)
                || await _db.Matriculas.AnyAsync(m => m.CarreraId == id);

            if (tieneRelaciones)
            {
                TempData["Error"] = $"No se puede eliminar la carrera {carrera.Nombre} porque tiene cursos o estudiantes asociados.";
                return RedirectToAction(nameof(Index));
            }

            // La relación con docentes se elimina primero.
            var docentes = await _db.DocentesCarreras.Where(dc => dc.CarreraId == id).ToListAsync();
            docentes.ForEach(dc => _db.DocentesCarreras.Remove(dc));

            _db.Carreras.Remove(carrera);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"La carrera {carrera.Nombre} se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
