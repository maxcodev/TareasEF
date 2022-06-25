using System.Configuration;
using EntityFramework;
using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<TareasContext>(o => o.UseInMemoryDatabase("TareasDB"));

//builder.Services.AddSqlServer<TareasContext>("Data Source=localhost; Initial Catalog=TareasDb; user id =sa; password=12m08s95c@@");
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("ServerConnectionMSSQL"));
// builder.Services.AddDbContext<TareasContext>(options =>
// {
//     var connectionString = builder.Configuration.GetConnectionString("ServerConnectionMySQL");
//     options.UseMySQL(connectionString, )
// });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/dbconexion", async ([FromServices] TareasContext DbContext) =>
{
    DbContext.Database.EnsureCreated();
    return Results.Ok("Base de Datos en memoria: " + DbContext.Database.IsInMemory());
});

app.MapGet("/api/tareas", async ([FromServices] TareasContext DbContext) =>
{
    return Results.Ok(DbContext.Tareas.Include(t => t.Categoria));
});

app.MapPost("/api/tareas", async ([FromServices] TareasContext DbContext, [FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await DbContext.AddAsync(tarea);

    await DbContext.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext DbContext, [FromForm] Tarea tarea, [FromRoute] Guid id) =>
{
    var TareaActual = DbContext.Tareas.Find(id);

    if (TareaActual != null)
    {
        TareaActual.CategoriaId = tarea.CategoriaId;
        TareaActual.Titulo = tarea.Titulo;
        TareaActual.PrioridadTarea = tarea.PrioridadTarea;
        TareaActual.Descripcion = tarea.Descripcion;

        await DbContext.SaveChangesAsync();
        return Results.Ok();
    }
        
    return Results.NotFound();
    
});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext DbContext, [FromRoute] Guid id) =>
{
    var TareaActual = DbContext.Tareas.Find(id);

    if (TareaActual != null)
    {
        DbContext.Remove(TareaActual);

        await DbContext.SaveChangesAsync();
        return Results.Ok();
    }
        
    return Results.NotFound();
});

app.Run();
