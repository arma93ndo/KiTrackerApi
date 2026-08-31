using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Context;

namespace KiTrackerApi.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    // Aquí se enlistan los distintos repositorios que controlará esta Unit of Work.
    public ILuchadorRepository Luchadores { get; }
    public ILecturaRepository Lecturas { get; }
    public IDispositivoRepository Dispositivos { get; }
    public IRepository<Especie> Especies { get; }
    public IRepository<Color> Colores { get; }
    

    public UnitOfWork(ApplicationDbContext context, ILuchadorRepository luchadores, IDispositivoRepository dispositivos,
        ILecturaRepository lecturas, IRepository<Especie> especies, IRepository<Color> colores)
    {
        // Se crea un solo DbContext, todos los repositorios usan ese mismo contexto. 
        // No hay múltiples conexiones en ningún momento.
        _context = context;
        Luchadores = luchadores;
        Dispositivos = dispositivos;
        Lecturas = lecturas;
        Especies = especies;
        Colores = colores;
    }

    // Guarda todos los cambios efectuados en los repositorios en una sola transacción (aislamiento, atomicidad) de SQL.
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    // Patrón IDispose de C# (diferente del "Disposable", es más simple). Libera la memoria del contexto (ApplicationDbContext) al finalizar la solicitud HTTP (Scoped lifecycle).
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}