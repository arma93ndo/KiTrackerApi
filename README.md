==================================================================================================================================
KiTrackerApi (a 10 de octubre de 2026)
==================================================================================================================================
Este proyecto se trata de una minimal API desarrollada con C#, .NET Core 10.0, SQLite, Entity Framework Core y xUnit.
Este proyecto se trata de una API RESTful mínima (ya que carece de Controladores) la cuál sigue un patrón arquitectónico monolítico con N-Tier.
En este caso, esta aplicación cuenta con 4 capas (o proyectos):
- Presentación: API.
- Core: Todo lo relacionado a la lógica de negocio nuclear.
- Data: La capa de acceso a base de datos.
- Tests: Involucra a las clases que ponen a prueba la funcionalidad de la aplicación (Unit tests, integration tests).

Esta aplicación hace uso de buenas prácticas y patrones de diseño que mejoran su legibilidad y mantenibilidad a lo largo del tiempo como son los patrones por inyección de dependencias
(DI), Unit of Work (UoW), repositorios, Data Transfer Objects (DTOs), manejo global de errores (middleware), uso de formatos y estándares como OpenAPI, Problem Details, etc.

==================================================================================================================================
Instalación
==================================================================================================================================
Es muy sencillo poder echar a andar esta solución. Sencillamente necesitas clonar su repositorio en alguna carpeta local dentro de tu máquina.
Dado que hace uso de una base de datos embebida (SQLite), no hace falta configurar cadenas de conexión ni inicializar instancias locales a sistemas gestores de bases de datos relacionales
(RDBMS) complicados. Además, esta aplicación cuenta con su propio sembrador de base de datos, para que ni siquiera tengas que preocuparte por tener datos válidos en la BBDD. Este ya
iniciliza la base de datos con datos de prueba para que puedas comenzar a manipularla desde el primer momento.
Sencillamente desde tu terminal (bash recomendada) posicionate en la ubicación del repositorio que clonaste en local y ejecuta lo siguiente:
# Ubicate en el proyecto por defecto (de presentación) de la aplicación.
cd KiTrackerApi/
# Inicializa el servicio de secretos de usuario (User secrets) ya que es requerido por los tokens JWT.
dotnet user-secrets init
# Definir el secreto específico que requieren los JWT para funcionar.
dotnet user-secrets set "Jwt:Clave" "esta-es-una-clave-de-desarrollo-de-al-menos-32-caracteres"
# Compila la totalidad de la solución.
dotnet build
# Ejecuta la solución.
dotnet run

A partir de este momento, el servidor (Kestrel) de la minimal API estará escuchando por nuevas peticiones (requests) por parte de clientes.
Para asegurar de que el proyecto se está ejecutando de forma satisfactoria, desde un navegador puedes visitar la siguiente liga: http://localhost:5111/
Deberás ver un mensaje como el siguiente impreso en pantalla:
"Hello World!"

Te recomiendo que pongas a prueba la salud de la solución al estarse ejecutando, enviando una solicitud de "Health check" a la misma. Para ello, utiliza cualquier terminal de tu preferencia
(en este caso usaré bash) e ingresa el siguiente comando para mandar una solicud de health check al servidor:
curl -X GET http://localhost:5111/health
Esta prueba de salud, verifica que los siguientes componentes estén operativos:
- El servidor Web (Kestrel).
- La base de datos (SQLite).
- Suficiencia de espacio del disco duro.
En caso de que todos estos componentes estén responsivos, deberías recibir una respuesta como esta:
"Healthy"

Puedes hacer uso de cualquier recurso de tu preferencia para poner a prueba los distintos recursos y endpoints de esta API (como son Postman o curl, o incluso, el propio archivo .http incluido
en la raíz del repositorio clonado). Si quieres conocer los distintos servicios y endpoints ofrecidos por esta API, dado que esta hace uso del estándar OpenAPI y de la herramienta gráfica Scalar,
te recomiendo que visites su endpoint específico para poder visualizar todos los endpoints ofrecidos: http://localhost:5111/scalar
Además, este servicio (Scalar) te ofrece facilidades para la prueba de los distintos endpoints de la API (scripts pre-hechos).

Si es de tu interés poner a prueba la aplicación y ejecutar los distintos unit tests incluidos en esta, es tan sencillo como regresar a tu terminal (bash recomendada) a la misma ubicación raíz 
del repositorio que clonaste (donde se ubica el archivo .slnx) e ingresar el siguiente comando:
# Ejecución de todos los tests incluidos en el proyecto de prueba "KiTrackerApi.Tests".
dotnet test

Con ese sencillo comando, se ejecutarán todos los tests existentes en la solución.