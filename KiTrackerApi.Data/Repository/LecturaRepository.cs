using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class LecturaRepository : Repository<Lectura>, ILecturaRepository
{
    public LecturaRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<IEnumerable<Lectura>> GetByEspecieIdAsync(int especieId)
    {
        return await _dbSet.AsNoTracking()
                        .Include(l => l.Luchador)
                            .ThenInclude(luchador => luchador!.Especie)
                        .Where(l => l.Luchador!.EspecieId == especieId)
                        .OrderByDescending(l => l.Luchador!.Especie!.Descripcion)
                        .ToListAsync();
    }

    public async Task<Lectura?> GetByIdConDetallesAsync(int id)
    {
        return await _dbSet
                    .Include(l => l.Luchador)
                        .ThenInclude(luchador => luchador!.Especie)
                    .Include(l => l.Dispositivo)
                    .FirstOrDefaultAsync(l => l.Id == id);
                            
    }

    public async Task<IEnumerable<Lectura>> GetByLuchadorIdAsync(int luchadorId)
    {
        return await _dbSet.Where(l => l.LuchadorId == luchadorId)
                            .OrderByDescending(l => l.FechaLectura)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Lectura>> GetByLuchadorIdConDetallesAsync(int luchadorId)
    {
        return await _dbSet.AsNoTracking()
                            .Include(l => l.Luchador)
                                .ThenInclude(luchador => luchador!.Especie)
                            .Where(l => l.LuchadorId == luchadorId)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Lectura>> GetLecturasKiInsideRango(long minimo, long maximo)
    {
        return await _dbSet.AsNoTracking()
                            .Include(l => l.Luchador)
                                .ThenInclude(luchador => luchador!.Especie)
                            .Where(l => l.NivelKi >= minimo && l.NivelKi <= maximo)
                            .OrderByDescending(l => l.NivelKi)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Lectura>> GetTodasConDetallesAsync()
    {
        return await _dbSet.Include(l => l.Luchador)
                                .ThenInclude(luchador => luchador!.Especie)
                            .OrderByDescending(l => l.Luchador!.Nombre)
                            .ToListAsync();
    }

    // Obtiene las N lecturas con los niveles de Ki más altos en el sistema
    public async Task<IEnumerable<Lectura>> GetTopKiLecturasAsync(int top)
    {
        return await _dbSet.AsNoTracking()
                            .Include(l => l.Luchador)
                                .ThenInclude(luchador => luchador!.Especie)
                            .OrderByDescending(l => l.NivelKi)
                            .Take(top)
                            .ToListAsync();
    }

    public async Task<Lectura?> GetUltimaLecturaByLuchadorIdAsync(int luchadorId)
    {
        return await _dbSet.Where(l => l.LuchadorId == luchadorId)
                            .OrderByDescending(l => l.FechaLectura)
                            .FirstOrDefaultAsync();
    }
}