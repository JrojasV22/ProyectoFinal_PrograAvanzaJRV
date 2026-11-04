using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.Models;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Página de entrada: envía a cada usuario a la pantalla que le corresponde según su rol.
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (User.IsInRole(Roles.Estudiante))
            {
                return RedirectToAction("Inicio", "Estudiante");
            }

            if (User.IsInRole(Roles.Administrador))
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        // Lista pública de las carreras disponibles con la cantidad de cursos de cada una.
        public async Task<IActionResult> Carreras()
        {
            var carreras = await _db.Carreras
                .Include(c => c.Cursos)
                .OrderBy(c => c.Nombre)
                .AsNoTracking()
                .ToListAsync();

            return View(carreras);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
