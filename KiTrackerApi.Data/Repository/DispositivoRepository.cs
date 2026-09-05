using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class DispositivoRepository : Repository<Dispositivo>, IDispositivoRepository
{
    public DispositivoRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<IEnumerable<Dispositivo>> GetByColorIdConDetallesAsync(int colorId)
    {
        return await _dbSet.Include(d => d.Color)
                        .Where(d => d.ColorId == colorId)
                        .OrderBy(d => d.Tipo)
                        .ToListAsync();
    }

    // Busca un dispositivo por su código único (Fingerprint).
    public Task<Dispositivo?> GetByFingerprintConDetallesAsync(string fingerprint)
    {
        return _dbSet.Include(d => d.Color)
                        .FirstOrDefaultAsync(d => d.Fingerprint == fingerprint);
                        
    }

    public Task<Dispositivo?> GetByIdConDetallesAsync(int id)
    {
        return _dbSet.Include(d => d.Color)
                        .FirstOrDefaultAsync(d => d.Id == id);
    }

    // Marca la entidad dispositivo como modificada en el Change Tracker de EF Core.
    public void Update(Dispositivo dispositivo)
    {
        _dbSet.Update(dispositivo);
    }

    public async Task<IEnumerable<Dispositivo>> GetAllConDetallesAsync()
    {
        return await _dbSet.Include(d => d.Color)
                        .OrderBy(d => d.Tipo)
                        .ToListAsync();
    }
}
