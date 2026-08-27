using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface ILecturaRepository : IRepository<Lectura>
{
    // Obtiene todas las lecturas registradas para un luchador en específico.
    Task<IEnumerable<Lectura>> GetLecturasByLuchadorIdAsync(int luchadorId);
    // Obtiene la lectura más reciente hecha a algún luchador.
    Task<Lectura?> GetUltimaLecturaByLuchadorIdAsync(int luchadorId); // "Lectura?" por si el luchador no tiene ninguna registrada.
    // Obtiene una lectura específica, cargando sus propiedades de navegación (Luchador, Especie, Dispositivo).
    Task<Lectura?> GetByIdConDetallesAsync(int id); // "Lectura?" por si el luchador no tiene ninguna registrada.
    // Obtiene las N lecturas con el nivel de Ki más alto registradas en el sistema.
    Task<IEnumerable<Lectura>> GetTopKiLecturasAsync(int top);
}