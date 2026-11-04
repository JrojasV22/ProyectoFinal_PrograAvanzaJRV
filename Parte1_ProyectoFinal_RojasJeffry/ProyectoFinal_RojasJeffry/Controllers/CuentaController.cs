using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Data;
using ProyectoFinal_RojasJeffry.Models;
using ProyectoFinal_RojasJeffry.ViewModels;

namespace ProyectoFinal_RojasJeffry.Controllers
{
    
    [AllowAnonymous]
    public class CuentaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CuentaController(ApplicationDbContext db,
                                UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---------- Registro de estudiante ----------

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            await CargarCarrerasAsync();
            return View(new RegistroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            // Validaciones del servidor que no se pueden hacer con atributos.
            if (await _db.Estudiantes.AnyAsync(e => e.Cedula == modelo.Cedula))
            {
                ModelState.AddModelError(nameof(modelo.Cedula), "Ya existe un estudiante registrado con esa cédula.");
            }

            if (await _userManager.FindByEmailAsync(modelo.Correo) != null)
            {
                ModelState.AddModelError(nameof(modelo.Correo), "Ese correo ya está registrado.");
            }

            if (modelo.CarreraId.HasValue && !await _db.Carreras.AnyAsync(c => c.Id == modelo.CarreraId))
            {
                ModelState.AddModelError(nameof(modelo.CarreraId), "La carrera seleccionada no existe.");
            }

            if (!ModelState.IsValid)
            {
                await CargarCarrerasAsync();
                return View(modelo);
            }

            // 1. Crear el usuario en Identity (credenciales).
            var usuario = new IdentityUser { UserName = modelo.Correo, Email = modelo.Correo };
            var resultado = await _userManager.CreateAsync(usuario, modelo.Contrasena);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await CargarCarrerasAsync();
                return View(modelo);
            }

            // 2. Asignar el rol Estudiante.
            await _userManager.AddToRoleAsync(usuario, Roles.Estudiante);

            // 3. Guardar los datos académicos del estudiante con su carrera.
            _db.Estudiantes.Add(new Estudiante
            {
                Cedula = modelo.Cedula,
                NombreCompleto = modelo.NombreCompleto,
                Correo = modelo.Correo,
                CarreraId = modelo.CarreraId!.Value,
                UsuarioId = usuario.Id
            });
            await _db.SaveChangesAsync();

            TempData["Exito"] = "Su cuenta se creó correctamente. Ya puede iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }

        // ---------- Inicio de sesión ----------

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _userManager.FindByEmailAsync(modelo.Correo);
            var claveCorrecta = usuario != null
                && (await _signInManager.CheckPasswordSignInAsync(usuario, modelo.Contrasena, lockoutOnFailure: false)).Succeeded;

            if (usuario == null || !claveCorrecta)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
                return View(modelo);
            }

            // Se agrega el nombre del estudiante a la sesión para mostrarlo en el menú.
            var claims = new List<Claim>();
            var estudiante = await _db.Estudiantes.AsNoTracking().FirstOrDefaultAsync(e => e.UsuarioId == usuario.Id);
            if (estudiante != null)
            {
                claims.Add(new Claim("NombreCompleto", estudiante.NombreCompleto));
            }

            await _signInManager.SignInWithClaimsAsync(usuario, modelo.Recordarme, claims);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Cada rol entra a su propia página de inicio.
            return RedirectToAction("Index", "Home");
        }

        // ---------- Cierre de sesión ----------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

        private async Task CargarCarrerasAsync()
        {
            var carreras = await _db.Carreras.OrderBy(c => c.Nombre).AsNoTracking().ToListAsync();
            ViewBag.Carreras = new SelectList(carreras, "Id", "Nombre");
        }
    }
}
