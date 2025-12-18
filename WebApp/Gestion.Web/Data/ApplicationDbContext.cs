using Microsoft.EntityFrameworkCore;
using Gestion.Web.Models; // Para acceder a Producto y Tarea

namespace Gestion.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor obligatorio para pasar las opciones de conexión
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets (tablas) para crear en la base de datos
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }

        // Controla que la FK ProductoId en Tarea esté indexada
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Producto) // Una Tarea tiene un Producto
                .WithMany(p => p.Tareas) // Un Producto tiene muchas Tareas
                .HasForeignKey(t => t.ProductoId); // La clave es ProductoId
        }
    }
}
