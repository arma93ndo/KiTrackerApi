using KiTrackerApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Interfaces.Repository;
using KiTrackerApi.Core.Models;
using KiTrackerApi.Data.Repository;
using KiTrackerApi.Data.Seed;
using KiTrackerApi.Core.Features.Especies;
using KiTrackerApi.Core.Features.Especies.DTOs;
using KiTrackerApi.Core.Features.Colores;
using KiTrackerApi.Errors;
using KiTrackerApi.Core.Features.Luchadores;
using KiTrackerApi.Core.Features.Dispositivos;
using KiTrackerApi.Core.Features.Lecturas;
using KiTrackerApi.Endpoints;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using KiTrackerApi.Services;
using Microsoft.Extensions.DependencyInjection;
using KiTrackerApi.Common.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro los repositorios en el contenedor IoC.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILuchadorRepository, LuchadorRepository>();
builder.Services.AddScoped<IDispositivoRepository, DispositivoRepository>();
builder.Services.AddScoped<ILecturaRepository, LecturaRepository>();
// Registro la Unit of Work (UoW) que dirige a los ya mencionados repositorios.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registro los servicios específicos de mis modelos.
builder.Services.AddScoped<IEspecieService, EspecieService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ILuchadorService, LuchadorService>();
builder.Services.AddScoped<IDispositivoService, DispositivoService>();
builder.Services.AddScoped<ILecturaService, LecturaService>();

// Registro el IProblemDetailsService. Todas las respuestas de error se formatearán según el
// formato estándar RFC 9457. Un JSON que contiene (por lo menos), las propiedades:
// - type
// - title
// - status
// - detail
// Agregaremos el TraceId. Este identificador aparecerá en los logs del servidor. El cliente
// podrá reportarnos este TraceId y nosotros podremos rastrear su petición exacta.
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = context => // CustomizeProblemDetails enriquece todas
    // las respuestas de error, de golpe. Como por arte de magia.
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
    }
);

// Registro mi middleware del manejador de errores global.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


// Registro el servicio de "health checks" (.AddHealthChecks()). El endpoint "/health" debería
// responder "Healthy" si es que la app está viva. Los servicios orquestadores y monitores lo
// utilizan para dar seguimiento a la salud de la aplicación (tiempo en línea u "uptime").
builder.Services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "database", 
                    tags: new[] { "db", "sqlite" }
                )
                .AddCheck<HealthCheckEspacioEnDisco>(
                    name: "espacio_disco",
                    tags: new[] { "sistema", "almacenamiento" });

// Registro el servicio de verificación de tokens JWT.
// Importante mencionar que los token y el sistema de usuarios son cosas separadas en una aplicación.
// Esta cpmfogiraco+pm de la API acepta y valida tokens JWT en las peticiones.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Obtengo la clave desde los User Secrets creada vía "dotnet usesr-jwts"
                    var signingKey = builder.Configuration["Jwt:Clave"]
                                    ?? builder.Configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"];
                    
                    if(string.IsNullOrWhiteSpace(signingKey))
                    {
                        throw new InvalidOperationException("Flata la clave de firma del JWT en la configuración (Secrets) de la aplicación.");
                    }
                    
                    // Decodifico la clave de Uer Secrets desde Base64 (fallback a UTF8 si usas una
                    // clave personalizada).
                    byte[] keyBytes;
                    try
                    {
                        keyBytes = Convert.FromBase64String(signingKey);
                    }
                    catch(FormatException)
                    {
                        keyBytes = Encoding.UTF8.GetBytes(signingKey);
                    }

                    // TokenValidationParameters: QUÉ se comprueba de cada token que llega. Sin estas
                    // validaciones, aceptar un token sería como confiar en un papel que cualquiera
                    // pudo haber escrito. Cada bandera cierra un agujero de seguridad.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ¿QUIÉN emitió el token? Sólo aceptamos los qque emitió esta misma API. Sin
                        // esto, un token válido de otro sistema que use la misma clave (o un emisor
                        // comprometido) entraría aquí sin restricciones.
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "dotnet-user-jwts",

                        // ¿Para QUIÉN es el token? Un token emitido para otra aplicación no debería
                        // servir en esta, aunque provenga de un emisor válido.
                        ValidateAudience = true,
                        ValidAudiences = new[] { builder.Configuration["Jwt:Audience"] ?? "http://localhost:5111" },

                        // ¿Está vigente? Comprueba el claim de expiración. Sin esto, la fecha de caduci-
                        // dad del token que pusimos sería meramente decorativa.
                        ValidateLifetime = true,

                        // Por defecto, .NET permite 5 minutos de desfase de reloj entre servidores. Lo
                        // bajamos a cero para que la expiración sea exacta. Un token vencido será
                        // rechazado al instante.
                        ClockSkew = TimeSpan.Zero,

                        // ¿La firma del token coincide con su contenido? La pieza más importante, impide
                        // que alguien edite un claim del payload (que recordemos, que es legible para
                        // cualquiera) y se haga pasar por otro usuario.
                        ValidateIssuerSigningKey = true,
                        // La clave, en caso de usarla, provendría de un User Secret en el caso del entorno
                        // de desarrollo. En el caso de un entorno de producción provendría de una variable
                        // de entorno o de Azure Key Vault. Pero en ningún caso provendría del archivo
                        // appsettings.json, ya que este último se sube y se versiona en el repositorio.
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });
builder.Services.AddAuthorization();

// Habilito el uso de ASP.NET Identity. Hago uso de .AddIdentityCore() y no de .AddIdentity(), ya que
// este último hace autenticación por cookies, cosa que necesita el uso de navegadores. Obviamente en
// esta minimal API no usamos navegadores ni tenemos vistas (.cshtml) de servidor. Esta minimal API
// sólo responde con JSONs brutos y autentica tokens JWT. .AddIdentityCore() trae lo que sí necesitamos:
// UserManager y el almacén de usuarios (tablas AspNet* de Identity).
builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    // Aquí definio las reglas que las contraseñas de los usuarios deben cumplir. Estas deberán ser de
    // por lo menos 8 caracteres, deben incluir mayúsculas, minúsculas, números y símbolos especiales.
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    // Dos usuarios no pueden compartir un email. Identity lo hace con un índice único (UNIQUE_INDEX) en
    // la BBDD, no se hace con un simple if. La garantía de unicidad del correo electrónico la da el
    // motor de BBDD.
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>(); // Esta línea le dice a Identity dónde guardar a los
// usuarios, que será nuestro propio contexto (ApplicationDbContext). Por eso es que nuestro contexto
// tuvo que heredar de IdentityDbContext, para que lo sepa usar.

// Registro el uso de mi servicio generador de tokens JWT.
builder.Services.AddScoped<JwtTokenService>();


var app = builder.Build();

#region BLOQUE DE PRUEBA REPOSITORIOS SIN ENDPOINTS HTTP
// // Borrar luego de probar.
// using(var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<ApplicationDbContext>();

//     await DbInitializer.SeedAsync(context);

//     // Inyectamos nuestros repositorios.
//     var luchadorRepo = services.GetRequiredService<ILuchadorRepository>();
//     var lecturaRepo = services.GetRequiredService<ILecturaRepository>();
//     var especieRepo = services.GetRequiredService<IRepository<Especie>>();
//     Console.WriteLine("🚀\n--- INICIANDO PRUEBA DE REPOSITORIOS ---");
//     // 1. Probar la inserción con Repository (vía herencia).
//     var especiesDisponibles = await especieRepo.GetAllAsync();
//     var especieUno = especiesDisponibles.First();

//     var nuevoLuchador = new Luchador
//     {
//         Nombre = "Goku",
//         EspecieId = especieUno.Id // Se asume que ya corrió el sembrador de Especies y Colores.
//     };

//     await luchadorRepo.AddAsync(nuevoLuchador);
//     await context.SaveChangesAsync(); // A falta de una Unit of Work (UoW), guardamos los cambios así.
//     Console.WriteLine($"✅ Luchador guardado con ID: {nuevoLuchador.Id}");

//     // 2. Probar método en específico (GetByIdConEspecieAsync).
//     var luchadorConSuEspecie = await luchadorRepo.GetByIdConEspecieAsync(nuevoLuchador.Id);
//     Console.WriteLine($"✅ Consultado: {luchadorConSuEspecie?.Nombre} - Especie: {luchadorConSuEspecie?.Especie?.Descripcion ?? "Sin Especie"}");

//     // 3. Probar método de lectura avanzada.
//     var lecturasTop = await lecturaRepo.GetTopKiLecturasAsync(5);
//     Console.WriteLine($"✅ Lecturas top encontradas: {lecturasTop.Count()}");

//     // 4. Consultar con .Include().
//     var luchadorGuardado = await luchadorRepo.GetByIdConEspecieAsync(nuevoLuchador.Id);
//     Console.WriteLine($"✅ Luchador: {luchadorGuardado?.Nombre} | Especie: {luchadorGuardado?.Especie?.Descripcion}");

//     Console.WriteLine($"🚀\n--- PRUEBA FINALIZADA CON ÉXITO ---\n");
// }
#endregion
#region BLOQUE DE PRUEBA UNIDAD DE TRABAJO SIN ENDPOINTS HTTP
// using(var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<ApplicationDbContext>();

//     // 1. Me aseguro de sembrar datos de prueba necesarios en la BBDD.
//     await DbInitializer.SeedAsync(context);

//     // 2. Resuelvo la dependencia al servicio de IUnitOfWork desde el contenedor IoC.
//     var unitOfWork = services.GetRequiredService<IUnitOfWork>();

//     Console.WriteLine("\n🚀 --- INICIANDO PRUEBA DE UNIT OF WORK ---");

//     // 3. Obtener una especie existente usando el repositorio genérico dentro de la UoW.
//     var especies = await unitOfWork.Especies.GetAllAsync();
//     var primeraEspecie = especies.First();

//     // 4. Crear un nuevo Luchador y almacenarlo mediante la UoW.
//     var nuevoLuchador = new Luchador
//     {
//         Nombre = "Gohan",
//         EspecieId = primeraEspecie.Id
//     };

//     await unitOfWork.Luchadores.AddAsync(nuevoLuchador);

//     // 5. Simulo una nueva operación (CRUD) en la misma transacción. Registro una nueva lectura.
//     // Primero necesito un dispositivo, creo uno.
//     var dispositivos = await unitOfWork.Dispositivos.GetAllAsync();
//     Dispositivo dispositivo;
//     var colores = await unitOfWork.Colores.GetAllAsync();
//     Color color;
//     if(!dispositivos.Any())
//     { // No hay ninguno, así que inserto uno nuevo.
//         color = colores.First();
//         dispositivo = new Dispositivo
//         {
//             Fingerprint = "SCOUTER-001",
//             Tipo = "mobile",
//             ModeloHardware = "iPhone 17",
//             ColorId = color.Id // Asumo que ya se ejecutó el seeder de la BBDD.
//         };
//         await unitOfWork.Dispositivos.AddAsync(dispositivo);
//         await unitOfWork.SaveChangesAsync(); // Guardo el dispositivo para obtener su ID (autogenerado por la BBDD).
//     }
//     else
//     {
//         // Ya existe alguno, así que lo reutilizo.
//         dispositivo = dispositivos.First();
//     }

//     // Registramos una lectura de Ki vinculada al nuevo luchador y al nuevo dispositivo.
//     var nuevaLectura = new Lectura
//     {
//         LuchadorId = nuevoLuchador.Id, // Estos IDs se asignan automáticamente al rastrear la entidad.
//         DispositivoId = dispositivo.Id,
//         NivelKi = 1500
//     };

//     await unitOfWork.Lecturas.AddAsync(nuevaLectura);

//     // 6. Confirmar (COMMIT) toda la transacción atómica con una sola llamada a .SaveChangesAsync();
//     var registrosAfectados = await unitOfWork.SaveChangesAsync();
//     Console.WriteLine($"✅ Transacción completada con éxito. Registros afectados en SQLite: {registrosAfectados}");

//     // 7. Comprobar que los datos se guardaron satisfactoriamente usando métodos especializados.
//     var luchadorRecuperado = await unitOfWork.Luchadores.GetByIdConEspecieAsync(nuevoLuchador.Id);
//     Console.WriteLine($"✅ Luchador recuperado: {luchadorRecuperado?.Nombre} (Especie: {luchadorRecuperado?.Especie?.Descripcion})");

//     var ultimaLectura = await unitOfWork.Lecturas.GetUltimaLecturaByLuchadorIdAsync(nuevoLuchador.Id);
//     Console.WriteLine($"✅ Última lectura de Ki registrada para ese luchador: {ultimaLectura?.NivelKi}");

//     Console.WriteLine("🚀 --- PRUEBA DE UNIT OF WORK FINALIZADA ---\n");
// }
#endregion
#region BLOQUE DE PRUEBA SERVICIO ESPECIES SIN ENDPOINTS HTTP
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<ApplicationDbContext>();

//     // 1. Siembro los datos mínimos necesarios como para una prueba vía mi sembrador.
//     await DbInitializer.SeedAsync(context);

//     // 2. Resolver la dependencia al servicio IEspecieService desde el contendor IoC.
//     var especieService = services.GetRequiredService<IEspecieService>();

//     Console.WriteLine("\n🚀 --- INICIANDO PRUEBAS DE ESPECIESERVICE ---");

//     // Prueba 1. Crear una especie sin enviar un multiplicador (deberá tomar el valor por omisión definido dentro del servicio).
//     var dtoSinMultiplicador = new CrearEspecieDto
//     {
//         Descripcion = "Bioandroide"
//     };
//     var respuesta1 = await especieService.CrearEspecieAsync(dtoSinMultiplicador);
//     Console.WriteLine($"✅ Test 1 (Sin Multiplicador) -> Creado ID: {respuesta1.Id}, Descripción: {respuesta1.Descripcion}");

//     // Prueba 2. Crear una especie enviando un multiplicador (debe ser diferente al valor por omisión).
//     var dtoConMultiplicador = new CrearEspecieDto
//     {
//         Descripcion = "Glindiana",
//         Multiplicador = 50.0
//     };
//     var respuesta2 = await especieService.CrearEspecieAsync(dtoConMultiplicador);
//     Console.WriteLine($"✅ Test 2 (Con Multiplicador) -> Creado ID: {respuesta2.Id}, Descripción: {respuesta2.Descripcion}");

//     // Prueba 3. Consultar todas las especies para verificar su persistencia en SQLite.
//     var todasLasEspecies = await especieService.ObtenerTodasAsync();
//     Console.WriteLine($"\n📋 Total de especies registradas en BD: {todasLasEspecies.Count()}");
//     foreach(EspecieRespuestaDto e in todasLasEspecies)
//     {
//         Console.WriteLine($" - ID: {e.Id} | {e.Descripcion}");
//     }

//     // Prueba 4. Actualizar una especie por Id.
//     var modificado = new ActualizarEspecieDto
//     {
//         Descripcion = "ayiya buba la tita",
//         Multiplicador = 3.0
//     };

//     bool prueba4 = await especieService.ActualizarEspecieByIdAsync(6, modificado);
//     Console.WriteLine($"Resultado de prueba 4: {prueba4}");

//     // Prueba 5. Actualizar una especie por su nombre.
//     var modificado2 = new ActualizarEspecieDto
//     {
//         Descripcion = "pustulio"    
//     };

//     bool prueba5 = await especieService.ActualizarEspecieByNombreAsync("glinDIANA", modificado2);
//     Console.WriteLine($"Resultado prueba 5: {prueba5}");

//     // Prueba 6. Eliminar una especie por Id.
//     bool prueba6 = await especieService.EliminarEspecieByIdAsync(6);
//     Console.WriteLine($"Resultado prueba 6: {prueba6}");

//     // Prueba 7. Eliminar una especie por su nombre.
//     bool prueba7 = await especieService.EliminarEspecieByNombreAsync("PUSTULIO");
//     Console.WriteLine($"Resultado prueba 7: {prueba7}");
    

//     Console.WriteLine("🚀 --- PRUEBAS FINALIZADAS CON ÉXITO ---\n");
// }
#endregion

using (var scope = app.Services.CreateScope())
{
    var servicios = scope.ServiceProvider;
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // Migrate()/MigrateAsync() aplica las migraciones pendientes de forma incremental, que es como
        // se maneja un esquema en un proyecto real.
        await db.Database.MigrateAsync();
    }
    catch(Exception ex)
    {
        var logger = servicios.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la inicialización de la base de datos. Stack Trace: {StackTrace}", ex.StackTrace);
    }
}

// El orden importa: el manejo de excepciones debe ir al principio del pipeline, para así envolver
// todo lo que ocurra después. .UseExceptionHandler() debe ir antes de .UseStatusCodePages().
#region ESPACIO RESERVADO PARA EL GLOBALEXCEPTIONHANDLER
// El Global Exception Handler (middleware) deberá ser el primero dentro del pipeline, para envolver
// todo lo que ocurra después.
app.UseExceptionHandler();
#endregion
app.UseStatusCodePages(); // Con esta línea, los códigos de error llegan al cliente rellenados
// con la información del estándar Problem Details. De lo contrario, llegarían al cliente como
// códigos de error completamente "pelones" (sin un body).

// CUIDADO: Invertir el siguiente par de líneas no tiene sentido. La autorización necesita saber quién
// eres para poder decidir. Sin .UseAuthentication delante, el usuario llegaría siempre anónimo y todo
// endpoint protegido respondería con un 401 (Unauthorized).
app.UseAuthentication(); // ¿Quién eres?. Lee el header "Authorization:" valida la firma y la expiración
// del token y arma al usuario de la petición (HttpContext.User).
app.UseAuthorization(); // ¿Puedes hacer eso?. Evalúa los requisitos del endpoint sobre ese usuario ya
// identificado.

// Este endpoint responderá 200 "Healthy" en caso de que la app esté viva ("up and running").
app.MapHealthChecks("/health");

// Expongo todos los endpoints necesarios en mi aplicación.
app.MapLuchadoresEndpoints();
app.MapDispositivosEndpoints();
app.MapLecturasEndpoints();
app.MapColoresEndpoints();
app.MapEspeciesEndpoints();
app.MapAuthEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();
