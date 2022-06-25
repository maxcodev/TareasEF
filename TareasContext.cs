using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

public class TareasContext : DbContext
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    
    public TareasContext(DbContextOptions<TareasContext> options) :base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba9d"), Nombre = "Actividades pendietes", Peso = 20});
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba02"), Nombre = "Actividades personales", Peso = 50});
        
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(c => c.CategoriaId);
            categoria.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(c => c.Descripcion).IsRequired(false);
            categoria.Property(c => c.Peso);

            categoria.HasData(categoriasInit);
        });

        List<Tarea> tareasInit = new List<Tarea>();
        tareasInit.Add(new Tarea(){ TareaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba10"), CategoriaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba9d"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios públicos", FechaCreacion = DateTime.Now});
        tareasInit.Add(new Tarea(){ TareaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba11"), CategoriaId = Guid.Parse("05d627dc-18d2-4719-a3b6-cf935f1bba02"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar ver pelicula en Netflix", FechaCreacion = DateTime.Now});
                
        
        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(t => t.TareaId);
            tarea.HasOne(t => t.Categoria).WithMany(t => t.Tareas).HasForeignKey(t => t.CategoriaId);
            tarea.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(t => t.Descripcion).IsRequired(false);
            tarea.Property(t => t.PrioridadTarea);
            tarea.Property(t => t.FechaCreacion);
            tarea.Ignore(t => t.Resumen);

            tarea.HasData(tareasInit);
        });
    }
}