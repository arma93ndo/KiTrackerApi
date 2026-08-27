using KiTrackerApi.Core.Models;
using KiTrackerApi.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class LuchadorRepository : Repository<Luchador>, ILuchadorRepository
{
    public LuchadorRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<IEnumerable<Luchador>> GetAllConEspecieAsync()
    {
        return await _dbSet.Include(l => l.Especie)
                            .ToListAsync();
    }

    public async Task<Luchador?> GetByIdConEspecieAsync(int id)
    {
        return await _dbSet.Include(l => l.Especie)
                            .FirstOrDefaultAsync(l => l.Id == id);
            
    }

    // Actualizar (Update).
    public void Update(Luchador luchador)
    {
        _dbSet.Update(luchador);

        // _db.SaveChangesAsync(); // De esto se encargará la Unit of Work (UoW).
    }
}