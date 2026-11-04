using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Models;

namespace ProyectoFinal_RojasJeffry.Data
{
    // Datos iniciales: roles, usuario administrador y catálogo de ejemplo.
    // Solo se insertan si las tablas están vacías, así que se puede ejecutar en cada arranque.
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider servicios)
        {
            var db = servicios.GetRequiredService<ApplicationDbContext>();
            var roleManager = servicios.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = servicios.GetRequiredService<UserManager<IdentityUser>>();
            var config = servicios.GetRequiredService<IConfiguration>();

            // Aplica las migraciones pendientes (crea la base de datos si no existe).
            await db.Database.MigrateAsync();

            // 1. Roles
            foreach (var rol in new[] { Roles.Administrador, Roles.Estudiante })
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // 2. Administrador inicial
            var correoAdmin = config["AdminInicial:Correo"] ?? "admin@matriculau.com";
            var claveAdmin = config["AdminInicial:Contrasena"] ?? "Admin123!";
            if (await userManager.FindByEmailAsync(correoAdmin) == null)
            {
                var admin = new IdentityUser { UserName = correoAdmin, Email = correoAdmin, EmailConfirmed = true };
                var resultado = await userManager.CreateAsync(admin, claveAdmin);
                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Administrador);
                }
            }

            // 3. Carreras
            if (!await db.Carreras.AnyAsync())
            {
                db.Carreras.AddRange(
                    new Carrera { Codigo = "ISC", Nombre = "Ingeniería en Sistemas de Computación", Descripcion = "Desarrollo de software y tecnologías de información" },
                    new Carrera { Codigo = "ADM", Nombre = "Administración de Empresas", Descripcion = "Gestión de organizaciones y negocios" },
                    new Carrera { Codigo = "CPA", Nombre = "Contaduría Pública", Descripcion = "Finanzas y contabilidad" },
                    new Carrera { Codigo = "IND", Nombre = "Ingeniería Industrial", Descripcion = "Procesos productivos y calidad" },
                    new Carrera { Codigo = "DGR", Nombre = "Diseño Gráfico", Descripcion = "Comunicación visual y diseño digital" },
                    new Carrera { Codigo = "DER", Nombre = "Derecho", Descripcion = "Ciencias jurídicas" });
                await db.SaveChangesAsync();
            }

            // 4. Docentes
            if (!await db.Docentes.AnyAsync())
            {
                db.Docentes.AddRange(
                    new Docente { Cedula = "1-1111-2222", NombreCompleto = "Ana Mora Solís", Correo = "amora@matriculau.com", Especialidad = "Desarrollo web" },
                    new Docente { Cedula = "2-3333-4444", NombreCompleto = "Carlos Jiménez Vega", Correo = "cjimenez@matriculau.com", Especialidad = "Bases de datos" },
                    new Docente { Cedula = "1-5555-6666", NombreCompleto = "Laura Castro Rojas", Correo = "lcastro@matriculau.com", Especialidad = "Programación" },
                    new Docente { Cedula = "3-7777-8888", NombreCompleto = "Diego Herrera Brenes", Correo = "dherrera@matriculau.com", Especialidad = "Redes" });
                await db.SaveChangesAsync();
            }

            // 5. Cursos (asociados a carreras; algunos con docente asignado)
            if (!await db.Cursos.AnyAsync())
            {
                var isc = await db.Carreras.FirstAsync(c => c.Codigo == "ISC");
                var adm = await db.Carreras.FirstAsync(c => c.Codigo == "ADM");
                var cpa = await db.Carreras.FirstAsync(c => c.Codigo == "CPA");
                var ana = await db.Docentes.FirstAsync(d => d.Cedula == "1-1111-2222");
                var carlos = await db.Docentes.FirstAsync(d => d.Cedula == "2-3333-4444");
                var laura = await db.Docentes.FirstAsync(d => d.Cedula == "1-5555-6666");
                var diego = await db.Docentes.FirstAsync(d => d.Cedula == "3-7777-8888");

                db.Cursos.AddRange(
                    new Curso { Codigo = "SC-302", Nombre = "Programación Avanzada en Web", Creditos = 4, Tipo = "Teórico-práctico", Horario = "Lunes 18:00 - 21:00", Cupo = 25, CarreraId = isc.Id, DocenteId = ana.Id },
                    new Curso { Codigo = "SC-401", Nombre = "Bases de Datos II", Creditos = 4, Tipo = "Teórico-práctico", Horario = "Martes 18:00 - 21:00", Cupo = 25, CarreraId = isc.Id, DocenteId = carlos.Id },
                    new Curso { Codigo = "SC-205", Nombre = "Estructuras de Datos", Creditos = 4, Tipo = "Teórico-práctico", Horario = "Miércoles 18:00 - 21:00", Cupo = 20, CarreraId = isc.Id, DocenteId = laura.Id },
                    new Curso { Codigo = "SC-310", Nombre = "Redes de Computadoras", Creditos = 3, Tipo = "Teórico-práctico", Horario = "Jueves 18:00 - 21:00", Cupo = 20, CarreraId = isc.Id, DocenteId = diego.Id },
                    new Curso { Codigo = "MA-210", Nombre = "Estadística", Creditos = 3, Tipo = "Teórico", Horario = "Sábado 08:00 - 11:00", Cupo = 30, CarreraId = isc.Id },
                    new Curso { Codigo = "AD-101", Nombre = "Fundamentos de Administración", Creditos = 3, Tipo = "Teórico", Horario = "Lunes 18:00 - 21:00", Cupo = 30, CarreraId = adm.Id },
                    new Curso { Codigo = "AD-205", Nombre = "Mercadeo", Creditos = 3, Tipo = "Teórico", Horario = "Miércoles 18:00 - 21:00", Cupo = 30, CarreraId = adm.Id },
                    new Curso { Codigo = "CP-101", Nombre = "Contabilidad I", Creditos = 4, Tipo = "Teórico-práctico", Horario = "Martes 18:00 - 21:00", Cupo = 30, CarreraId = cpa.Id });

                db.DocentesCarreras.AddRange(
                    new DocenteCarrera { DocenteId = ana.Id, CarreraId = isc.Id },
                    new DocenteCarrera { DocenteId = carlos.Id, CarreraId = isc.Id },
                    new DocenteCarrera { DocenteId = laura.Id, CarreraId = isc.Id },
                    new DocenteCarrera { DocenteId = diego.Id, CarreraId = isc.Id });

                await db.SaveChangesAsync();
            }
        }
    }
}
