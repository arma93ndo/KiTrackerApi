using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class DispositivoRepository : Repository<Dispositivo>, IDispositivoRepository
{
    public DispositivoRepository(ApplicationDbContext context) : base(context)
    {}

    // Busca un dispositivo por su código único (Fingerprint).
    public Task<Dispositivo?> GetByFingerprintAsync(string fingerprint)
    {
        return _dbSet.Include(d => d.Color)
                        .FirstOrDefaultAsync(d => d.Fingerprint == fingerprint);
                        
    }

    // Marca la entidad dispositivo como modificada en el Change Tracker de EF Core.
    public void Update(Dispositivo dispositivo)
    {
        _dbSet.Update(dispositivo);
    }
}
