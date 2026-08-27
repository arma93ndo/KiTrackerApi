using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface IDispositivoRepository : IRepository<Dispositivo>
{
    // Obtiene un dispositivo por su huella dactilar (fingerprint).
    Task<Dispositivo?> GetByFingerprintAsync(string fingerprint);
    // Marca un dipositivo para actualizar en el Change Tracker de EF Core.
    void Update(Dispositivo dispositivo);
}