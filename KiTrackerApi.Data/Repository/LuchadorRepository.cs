using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Data.Context;
using KiTrackerApi.Core.Extensions;

namespace KiTrackerApi.Data.Repository;

public class LuchadorRepository : Repository<Luchador>, ILuchadorRepository
{
    public LuchadorRepository(ApplicationDbContext context) : base(context)
    {}

    // Lecturas (Read).
    public async Task<IEnumerable<Luchador>> GetAllConEspecieAsync()
    {
        return await _dbSet.AsNoTracking()
                            .Include(l => l.Especie)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Luchador>> GetByEspecieIdAsync(int especieId)
    {
        return await _dbSet.AsNoTracking()
                            .Include(l => l.Especie)
                            .Where(l => l.EspecieId == especieId)
                            .ToListAsync();
    }

    public async Task<Luchador?> GetByIdConEspecieAsync(int id)
    {
        return await _dbSet.Include(l => l.Especie)
                            .FirstOrDefaultAsync(l => l.Id == id);
            
    }

    public async Task<Luchador?> GetByNombreAsync(string nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre))
            return null;
        
        var limpio = nombre.SanitizarNombrePropio().ToLower();

        return await _context.Luchadores
            .FirstOrDefaultAsync(l => l.Nombre.ToLower() == limpio);
    }

    public async Task<Luchador?> GetByNombreConEspecieAsync(string nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre))
            return null;

        var limpio = nombre.SanitizarNombrePropio().ToLower();

        return await _context.Luchadores.Include(l => l.Especie)
            .FirstOrDefaultAsync(l => l.Nombre.ToLower() == limpio);
    }

    // Actualizar (Update).
    public void Update(Luchador luchador)
    {
        _dbSet.Update(luchador);

        // _db.SaveChangesAsync(); // De esto se encargará la Unit of Work (UoW).
    }
}