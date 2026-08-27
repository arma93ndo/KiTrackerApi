using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface ILuchadorRepository : IRepository<Luchador>
{
    Task<Luchador?> GetByIdConEspecieAsync(int id);
    Task<IEnumerable<Luchador>> GetAllConEspecieAsync();
    void Update(Luchador luchador); // Método específico para actualizar este modelo.
}