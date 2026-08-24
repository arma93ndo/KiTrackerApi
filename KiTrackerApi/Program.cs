using KiTrackerApi.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Migrate() aplica las migraciones pendientes de forma incremental, que es como
    // se maneja un esquema en un proyecto real.
    db.Database.Migrate();
}


app.MapGet("/", () => "Hello World!");

app.Run();
