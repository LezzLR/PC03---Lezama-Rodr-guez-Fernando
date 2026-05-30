using Microsoft.EntityFrameworkCore;
using GestionTareasApi.Models;

namespace GestionTareasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Guardar enums como strings en la base de datos para legibilidad
            modelBuilder.Entity<Tarea>()
                .Property(t => t.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Tarea>()
                .Property(t => t.Prioridad)
                .HasConversion<string>();
        }
    }
}
