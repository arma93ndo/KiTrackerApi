using KiTrackerApi.Data.Context;
using KiTrackerApi.Core.Models;

namespace KiTrackerApi.Data.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Obtengo el dbSet (la tabla) específico de mi modelo para empezar.
        var dbSet1 = context.Set<Especie>();

        // 1. Aseguro que la BBDD exista.
        await context.Database.EnsureCreatedAsync();

        // 2. Poblar la tabla Especie (en caso de estar vacía).
        if(!dbSet1.Any())
        {
            var especies = new List<Especie>
            {
                new Especie { Descripcion = "Humano" },
                new Especie {Descripcion = "Saiyajin"},
                new Especie {Descripcion = "Androide"},
                new Especie {Descripcion = "Raza de Freezer"},
                new Especie {Descripcion = "Namekuseijin"}
            };
            // Ingreso los registros recién creados al dbSet (tabla SQL).
            await dbSet1.AddRangeAsync(especies);
        }

        // Ahora opero con la tabla (modelo) Colores.
        var dbSet2 = context.Set<Color>();

        // 3. Poblar la tabla Colores (en caso de estar vacía).
        if(!dbSet2.Any())
        {
            var colores = new List<Color>
            {
                new Color { Descripcion = "Verde", CodigoHex = "1E5228" }, // Verde como el de Raditz.
                new Color { Descripcion = "Rojo", CodigoHex = "AF131C" }, // Rojo como el de Vegeta.
                new Color { Descripcion = "Azul", CodigoHex = "2732A4" } // Azul como el de Nappa.
            };

            await dbSet2.AddRangeAsync(colores);
        }

        // 4. Guardar los registros en SQLite.
        await context.SaveChangesAsync();
    }
}