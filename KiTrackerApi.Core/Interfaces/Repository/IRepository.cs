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

    // Patrón para poder hacer uso de una consulta IQueryable con el uso de Unit of Work sin violar la responsabilidad de las capas.
    IQueryable<T> ObtenerQueryable();
    Task<List<TResult>> MaterializarConsultaAsync<TResult>(IQueryable<TResult> consulta);
    Task<int> ContarConsultaAsync<TResult>(IQueryable<TResult> consulta);
}