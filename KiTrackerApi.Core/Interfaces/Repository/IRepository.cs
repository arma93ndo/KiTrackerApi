using System.Linq.Expressions;

namespace KiTrackerApi.Core.Interfaces.Repository;

public interface IRepository<T> where T : class
{
    // Las operaciones CRxD.
    // Lecturas (Read).
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // Escrituras (Create).
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    // Eliminación (Delete).
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}