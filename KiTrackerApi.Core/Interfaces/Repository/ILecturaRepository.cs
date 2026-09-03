using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface ILecturaRepository : IRepository<Lectura>
{
    // Obtiene todas las lecturas registradas para un luchador en específico.
    Task<IEnumerable<Lectura>> GetByLuchadorIdAsync(int luchadorId);
    // Obtiene todas las lecturas registradas para un luchar en específico incluyendo datos particulares como su Especie.
    Task<IEnumerable<Lectura>> GetByLuchadorIdConDetallesAsync(int luchadorId);
    // Obtiene la lectura más reciente hecha a algún luchador.
    Task<Lectura?> GetUltimaLecturaByLuchadorIdAsync(int luchadorId); // "Lectura?" por si el luchador no tiene ninguna registrada.
    // Obtiene una lectura específica, cargando sus propiedades de navegación (Luchador, Especie, Dispositivo).
    Task<Lectura?> GetByIdConDetallesAsync(int id); // "Lectura?" por si el luchador no tiene ninguna registrada.
    // Obtiene todas las lecturas relacionadas con la especie proporcionada.
    Task<IEnumerable<Lectura>> GetByEspecieIdAsync(int especieId);
    // Obtiene las N lecturas con el nivel de Ki más alto registradas en el sistema.
    Task<IEnumerable<Lectura>> GetTopKiLecturasAsync(int top);
    // Obtiene las lecturas de Ki que se hallen entre un máximo y un mínimo especificado [min, max].
    Task<IEnumerable<Lectura>> GetLecturasKiInsideRango(long minimo, long maximo);
    // Obtengo todas las lecturas en la tabla Lecturas sin excepción, aunadas a sus detalles.
    Task<IEnumerable<Lectura>> GetTodasConDetallesAsync();
}