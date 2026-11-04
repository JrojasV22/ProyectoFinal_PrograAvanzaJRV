using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.Models;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    // Registro de docentes: crear, listar, editar y eliminar (HU-12).
    [Authorize(Roles = Roles.Administrador)]
    public class DocentesController : Controller
    {
        private const int TamanoPagina = 10;
        private readonly ApplicationDbContext _db;

        public DocentesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Docentes
        public async Task<IActionResult> Index(int pagina = 1)
        {
            var consulta = _db.Docentes
                .Include(d => d.Cursos)
                .AsNoTracking()
                .OrderBy(d => d.NombreCompleto);

            var lista = await ListaPaginada<Docente>.CrearAsync(consulta, pagina, TamanoPagina);
            return View(lista);
        }

        // GET: /Docentes/Create
        public IActionResult Create()
        {
            return View(new Docente());
        }

        // POST: /Docentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cedula,NombreCompleto,Correo,Especialidad")] Docente docente)
        {
            await ValidarAsync(docente);

            if (!ModelState.IsValid)
            {
                return View(docente);
            }

            _db.Docentes.Add(docente);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"El docente {docente.NombreCompleto} se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Docentes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var docente = await _db.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }
            return View(docente);
        }

        // POST: /Docentes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cedula,NombreCompleto,Correo,Especialidad")] Docente docente)
        {
            if (id != docente.Id)
            {
                return NotFound();
            }

            await ValidarAsync(docente);

            if (!ModelState.IsValid)
            {
                return View(docente);
            }

            var existente = await _db.Docentes.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            existente.Cedula = docente.Cedula;
            existente.NombreCompleto = docente.NombreCompleto;
            existente.Correo = docente.Correo;
            existente.Especialidad = docente.Especialidad;
            await _db.SaveChangesAsync();

            TempData["Exito"] = $"El docente {docente.NombreCompleto} se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Docentes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var docente = await _db.Docentes
                .Include(d => d.Cursos)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (docente == null)
            {
                return NotFound();
            }
            return View(docente);
        }

        // POST: /Docentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _db.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }

            // No se puede eliminar un docente que tenga cursos asignados.
            if (await _db.Cursos.AnyAsync(c => c.DocenteId == id))
            {
                TempData["Error"] = $"No se puede eliminar a {docente.NombreCompleto} porque tiene cursos asignados.";
                return RedirectToAction(nameof(Index));
            }

            var carreras = await _db.DocentesCarreras.Where(dc => dc.DocenteId == id).ToListAsync();
            carreras.ForEach(dc => _db.DocentesCarreras.Remove(dc));

            _db.Docentes.Remove(docente);
            await _db.SaveChangesAsync();
            TempData["Exito"] = $"El docente {docente.NombreCompleto} se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidarAsync(Docente docente)
        {
            if (await _db.Docentes.AnyAsync(d => d.Cedula == docente.Cedula && d.Id != docente.Id))
            {
                ModelState.AddModelError(nameof(docente.Cedula), "Ya existe un docente con esa cédula.");
            }

            if (await _db.Docentes.AnyAsync(d => d.Correo == docente.Correo && d.Id != docente.Id))
            {
                ModelState.AddModelError(nameof(docente.Correo), "Ya existe un docente con ese correo.");
            }
        }
    }
}
