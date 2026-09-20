using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KiTrackerApi.Data.Context;

// Una buena práctica, es tener un solo DbContext por aplicación (solución).
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}

    // Lista de todos los DbSets (modelos) usados dentro de la aplicación:
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Luchador> Luchadores => Set<Luchador>();
    public DbSet<Lectura> Lecturas => Set<Lectura>();
    public DbSet<Dispositivo> Dispositivos => Set<Dispositivo>();
    public DbSet<Color> Colores => Set<Color>();

    // Métodos y sembrado de datos.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Llamo a mi método de sembrado de datos (estático).
        DbInitializer.SeedKiTrackerApiAsync(modelBuilder);
    }
}