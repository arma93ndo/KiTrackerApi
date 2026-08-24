using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Data.Context;

// Una buena práctica, es tener un solo DbContext por aplicación (solución).
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}

    // Lista de todos los DbSets (modelos) usados dentro de la aplicación:
    public DbSet<Especie> Especies => Set<Especie>();
    public DbSet<Luchador> Luchadores => Set<Luchador>();
    public DbSet<Lectura> Lecturas => Set<Lectura>();
    public DbSet<Dispositivo> Dispositivos => Set<Dispositivo>();
    public DbSet<Color> Colores => Set<Color>();
}