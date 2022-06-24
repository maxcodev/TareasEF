using System.Configuration;
using EntityFramework;
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
app.Run();
