using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_RojasJeffry.Models;

namespace ProyectoFinal_RojasJeffry.Data
{
   
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Carrera> Carreras => Set<Carrera>();
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<Docente> Docentes => Set<Docente>();
        public DbSet<DocenteCarrera> DocentesCarreras => Set<DocenteCarrera>();
        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Matricula> Matriculas => Set<Matricula>();
        public DbSet<DetalleMatricula> DetallesMatricula => Set<DetalleMatricula>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //se usa Restrict: no se puede borrar un registro
            // que tenga otros registros relacionados (por ejemplo, una carrera con cursos).
            var relaciones = builder.Model.GetEntityTypes()
                .Where(e => e.ClrType.Namespace == typeof(Carrera).Namespace)
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relacion in relaciones)
            {
                relacion.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
