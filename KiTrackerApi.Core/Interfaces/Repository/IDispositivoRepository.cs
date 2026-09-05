using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface IDispositivoRepository : IRepository<Dispositivo>
{
    // Obtiene un dispositivo por Id con sus datos relacionados.
    Task<Dispositivo?> GetByIdConDetallesAsync(int id);
    // Obtiene un dispositivo por su huella dactilar (fingerprint) junto con sus datos relacionados.
    Task<Dispositivo?> GetByFingerprintConDetallesAsync(string fingerprint);
    // Marca un dipositivo para actualizar en el Change Tracker de EF Core.
    void Update(Dispositivo dispositivo);
    // Obtiene todos los dispositivos con un color determinado.
    Task<IEnumerable<Dispositivo>> GetByColorIdConDetallesAsync(int colorId);
    // Obtengo todas las instancias con sus detalles poblados.
    Task<IEnumerable<Dispositivo>> GetAllConDetallesAsync();
}