using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class LecturaRepository : Repository<Lectura>, ILecturaRepository
{
    public LecturaRepository(ApplicationDbContext context) : base(context)
    {}
    public async Task<Lectura?> GetByIdConDetallesAsync(int id)
    {
        return await _dbSet
                    .Include(l => l.Luchador)
                        .ThenInclude(luchador => luchador!.Especie)
                    .Include(l => l.Dispositivo)
                    .FirstOrDefaultAsync(l => l.Id == id);
                            
    }

    public async Task<IEnumerable<Lectura>> GetLecturasByLuchadorIdAsync(int luchadorId)
    {
        return await _dbSet.Where(l => l.LuchadorId == luchadorId)
                            .OrderByDescending(l => l.FechaLectura)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Lectura>> GetTopKiLecturasAsync(int top)
    {
        return await _dbSet.Include(l => l.Luchador)
                            .OrderByDescending(l => l.NivelKi)
                            .Take(top)
                            .ToListAsync();
    }

    // Obtiene las N lecturas con los niveles de Ki más altos en el sistema
    public async Task<Lectura?> GetUltimaLecturaByLuchadorIdAsync(int luchadorId)
    {
        return await _dbSet.Where(l => l.LuchadorId == luchadorId)
                            .OrderByDescending(l => l.FechaLectura)
                            .FirstOrDefaultAsync();
    }
}