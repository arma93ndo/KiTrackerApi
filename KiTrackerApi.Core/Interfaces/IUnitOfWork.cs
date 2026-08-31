using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Interfaces.Repository;

namespace KiTrackerApi.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositorios específicos.
    ILuchadorRepository Luchadores { get; }
    ILecturaRepository Lecturas { get; }
    IDispositivoRepository Dispositivos { get; }
    // Repositorios genéricos para modelos que no requiren de métodos especiales (p. ej. los catálogos)
    IRepository<Especie> Especies { get; }
    IRepository<Color> Colores { get; }

    Task<int> SaveChangesAsync(); // Se usará para persistir los datos hechos al contexto.
}