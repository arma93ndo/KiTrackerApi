using KiTrackerApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Repository;
using KiTrackerApi.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro los repositorios en el contenedor IoC.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILuchadorRepository, LuchadorRepository>();
builder.Services.AddScoped<IDispositivoRepository, DispositivoRepository>();
builder.Services.AddScoped<ILecturaRepository, LecturaRepository>();


var app = builder.Build();

#region BLOQUE DE PRUEBA SIN ENDPOINTS HTTP
// Borrar luego de probar.
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    await DbInitializer.SeedAsync(context);
    Console.WriteLine("🌱 BBDD sembrada con Especies y Colores satisfactoriamente.");

    // // Se asegura de que la BBDD de SQLite exista y tenga las tablas pertinentes.
    // await context.Database.EnsureCreatedAsync();

    // // Inyectamos nuestros repositorios.
    // var luchadorRepo = services.GetRequiredService<ILuchadorRepository>();
    // var lecturaRepo = services.GetRequiredService<ILecturaRepository>();
    // Console.WriteLine("--- INICIANDO PRUEBA DE REPOSITORIOS ---");
    // // 1. Probar la inserción con Repository (vía herencia).
    // var nuevoLuchador = new Luchador
    // {
    //     Nombre = "Goku",
    //     EspecieId = 1;
    // };
}
#endregion


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Migrate() aplica las migraciones pendientes de forma incremental, que es como
    // se maneja un esquema en un proyecto real.
    db.Database.Migrate();
}


app.MapGet("/", () => "Hello World!");

app.Run();
