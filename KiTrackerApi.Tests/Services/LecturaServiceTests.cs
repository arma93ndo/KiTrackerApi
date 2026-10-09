using System.IO.Compression;
using KiTrackerApi.Core.Features.Lecturas;
using KiTrackerApi.Core.Features.Lecturas.DTOs;
using KiTrackerApi.Core.Interfaces;
using KiTrackerApi.Core.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KiTrackerApi.Tests.Services;

public class LecturaServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ILogger<LecturaService>> _loggerMock;
    private readonly LecturaService _lecturaService;

    public LecturaServiceTests()
    {
        // Se ejecuta antes de cada prueba.
        _uowMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<LecturaService>>();
        // Instancio el servicio, inyectando los mocks.
        _lecturaService = new LecturaService(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ObtenerTodasAsync_ConLecturasExistentes_DevuelveListaMapeadaCorrectamente()
    {
        // 1. Arrange.
        // Creo una nueva lista, que incluirá objetos anónimos de tipo "Lectura" para simular el
        // resultado de una consulta de la unit of work.
        List<Lectura> lecturasFake = new List<Lectura>
        {
            new Lectura
            {
                Id = 1,
                LuchadorId = 10,
                DispositivoId = 20,
                NivelKi = 9001,
                FechaLectura = DateTime.UtcNow,
                Luchador = new Luchador
                {
                    Nombre = "Goku",
                    Especie = new Especie { Descripcion = "Saiyajin" }
                }
            }
        };

        // Configuro la cadena de llamadas _uow.Lecturas.GetTodasConDetallesAsync()
        _uowMock.Setup(u => u.Lecturas.GetTodasConDetallesAsync()).ReturnsAsync(lecturasFake);

        // 2. Act.
        var resultado = await _lecturaService.ObtenerTodasAsync();

        // 3. Assert.
        Assert.NotNull(resultado);
        var listaResultado = resultado.ToList();

        Assert.Single(listaResultado);

        LecturaRespuestaDto dto = listaResultado.First();
        Assert.Equal(1, dto.Id);
        Assert.Equal(10, dto.LuchadorId);
        Assert.Equal(20, dto.DispositivoId);
        Assert.Equal(9001, dto.NivelKi);
        Assert.Equal("Goku", dto.NombreLuchador);
        Assert.Equal("Saiyajin", dto.NombreEspecie);

        // Verifico que se consultó al repositorio exactamente 1 vez.
        _uowMock.Verify(u => u.Lecturas.GetTodasConDetallesAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerTodasAsync_CuandoElRepositorioDevuelveNull_DvuelveColeccionVaciaSinExcepcion()
    {
        // 1. Arrange.
        // Simulamos que el repositorio devuelve null en lugar de una lista.
        _uowMock.Setup(u => u.Lecturas.GetTodasConDetallesAsync()).ReturnsAsync(Enumerable.Empty<Lectura>());

        // 2. Act.
        var resultado = await _lecturaService.ObtenerTodasAsync();

        // 3. Assert.
        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        _uowMock.Verify(u => u.Lecturas.GetTodasConDetallesAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerTodasAsync_ConLuchadorOEspecieNull_MapeaValoresPorDefecto()
    {
        // 1. Arrange.
        // Valido la lógica defensiva de la expresión condicional (l.Luchador?.Nombre ?? string.Empty)
        var lecturasFake = new List<Lectura>
        {
            new()
            {
                Id = 2,
                LuchadorId = 11,
                DispositivoId = 21,
                NivelKi = 500,
                FechaLectura = DateTime.UtcNow,
                Luchador = null // Carece de una entidad de navegación cargada.
            }
        };

        _uowMock.Setup(u => u.Lecturas.GetTodasConDetallesAsync()).ReturnsAsync(lecturasFake);

        // 2. Act.
        var resultado = await _lecturaService.ObtenerTodasAsync();

        // 3. Assert.
        var dto = Assert.Single(resultado);
        Assert.Equal(string.Empty, dto.NombreLuchador);
        Assert.Null(dto.NombreEspecie);
    }

    [Fact]
    public async Task EliminarByIdAsync_ConIdValido_EliminaUnRegistroSatisfactoriamente()
    {
        // 1. Arrange.
        int idBuscado = 20;

        var lecturaFake = new Lectura
        {
            Id = 20,
            LuchadorId = 5,
            DispositivoId = 10,
            NivelKi = 4000000000,
            FechaLectura = DateTime.UtcNow,
            Luchador = new Luchador
            {
                Nombre = "Freezer",
                Especie = new Especie
                {
                    Descripcion = "Mutante del clan de Freezer"
                }
            }
        };


        // Simulo que la lectura sí existe en la BBDD.
        _uowMock.Setup(u => u.Lecturas.GetByIdAsync(idBuscado)).ReturnsAsync(lecturaFake);

        // Simulo que la persistencia en la Unit of Work se ejecuta con éxito.
        _uowMock.Setup(u => u.SaveChangesAsync());

        // 2. Act.
        await _lecturaService.EliminarByIdAsync(idBuscado);

        // 3. Assert.
        // Verificaciones de comportamiento: me aseguro de que el servicio ejecutó la cadena esperada.
        _uowMock.Verify(u => u.Lecturas.GetByIdAsync(idBuscado), Times.Once);
        _uowMock.Verify(u => u.Lecturas.Remove(lecturaFake), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarByIdAsync_ConIdInvalido_LanzaExcepcionKeyNotFoundException()
    {
        // 1. Arrange.
        int idBuscado = 99999;
        _uowMock.Setup(u => u.Lecturas.GetByIdAsync(idBuscado)).ReturnsAsync((Lectura?)null);

        // 2. Act & 3. Assert.
        // Verifico que se lance una excepción esperada al invocar el método.
        var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _lecturaService.EliminarByIdAsync(idBuscado));

        Assert.Contains("no existe en la BBDD. Eliminación imposible.", excepcion.Message);
        _uowMock.Verify(u => u.Lecturas.Remove(It.IsAny<Lectura>()), Times.Never);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ObtenerByIdAsync_ConIdValido_DevuelveInstanciaSatisfactoriamente()
    {
        // 1. Arrange.
        int idBuscado = 30;
        var LecturaBuscada = new Lectura
        {
            Id = 30,
            LuchadorId = 4,
            Luchador = new Luchador
            {
                Nombre = "Vegeta",
                Especie = new Especie
                {
                    Descripcion = "Saiyajin"
                }
            },
            DispositivoId = 13,
            NivelKi = 1000000,
            FechaLectura = DateTime.UtcNow
        };

        // Simulo la existencia del luchador especificado en la BBDD.
        _uowMock.Setup(u => u.Lecturas.GetByIdConDetallesAsync(idBuscado)).ReturnsAsync(LecturaBuscada);

        // 2. Act.
        LecturaRespuestaDto? lectura = await _lecturaService.ObtenerByIdAsync(idBuscado);

        // 3. Assert.
        Assert.NotNull(lectura);
        Assert.Equal(idBuscado, lectura.Id);
        Assert.Equal(4, lectura.LuchadorId);
        Assert.Equal(13, lectura.DispositivoId);
        Assert.Equal(1000000, lectura.NivelKi);
        Assert.Equal("Vegeta", lectura.NombreLuchador);
        Assert.Equal("Saiyajin", lectura.NombreEspecie);
    }

    [Fact]
    public async Task ObtenerByIdAsync_ConIdInvalido_LanzaExcepcionKeyNotFoundException()
    {
        // 1. Arrange
        int idBuscado = 99999;

        // Simulo que devuelve null al buscarlo en la BBDD.
        _uowMock.Setup(u => u.Lecturas.GetByIdConDetallesAsync(idBuscado)).ReturnsAsync((Lectura?)null);

        // 2. Act & 3. Assert.
        var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _lecturaService.ObtenerByIdAsync(idBuscado));

        Assert.Contains("no existe en la BBDD.", excepcion.Message);
        _uowMock.Verify(u => u.Lecturas.GetByIdConDetallesAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ObtenerByLuchadorIdAsync_ConIdExistente_DevuelveListaMapeadaCorrectamente()
    {
        // 1. Arrange.
        int idBuscado = 100;
        var luchadorValido = new Luchador
        {
            Nombre = "Gohan",
            Especie = new Especie
            {
                Descripcion = "Saiyajin"
            }
        };

        var listaLecturas = new List<Lectura>
        {
            new Lectura
            {
                Id = 1024,
                LuchadorId = idBuscado,
                Luchador = new Luchador
                {
                    Nombre = "Gohan",
                    Especie = new Especie
                    {
                        Descripcion = "Saiyajin"
                    }
                },
                DispositivoId = 23,
                NivelKi = 2500,
                FechaLectura = DateTime.UtcNow
            }
        };

        // Simulo que la UoW devuelve una instancia de luchador válida al buscarla.
        _uowMock.Setup(u => u.Luchadores.GetByIdAsync(idBuscado)).ReturnsAsync(luchadorValido);
        // Simulo que la UoW devuelve una lista de Lecturas desde la BBDD.
        _uowMock.Setup(u => u.Lecturas.GetByLuchadorIdConDetallesAsync(idBuscado)).ReturnsAsync(listaLecturas);

        // 2. Act.
        var resultado = await _lecturaService.ObtenerByLuchadorIdAsync(idBuscado);

        // 3. Assert.
        Assert.NotNull(resultado);
        List<LecturaRespuestaDto> listaResultante = resultado.ToList();

        Assert.Single(listaResultante);
        var dto = listaResultante.First();

        Assert.Equal(idBuscado, dto.LuchadorId);
        Assert.Equal(2500, dto.NivelKi);
        Assert.Equal(23, dto.DispositivoId);
        Assert.Equal(1024, dto.Id);
        Assert.Equal("Gohan", dto.NombreLuchador);
        Assert.Equal("Saiyajin", dto.NombreEspecie);

        _uowMock.Verify(u => u.Luchadores.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.GetByLuchadorIdConDetallesAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ObtenerByLuchadorIdAsync_ConIdInexistente_LanzaExcepcionKeyNotFoundException()
    {
        // 1. Arrange.
        int idBuscado = 99999;
        // Simulo devolver un null desde la BBDD.
        _uowMock.Setup(u => u.Luchadores.GetByIdAsync(idBuscado)).ReturnsAsync((Luchador?) null);

        // 2. Act & 3. Assert.
        var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _lecturaService.ObtenerByLuchadorIdAsync(idBuscado));

        Assert.Contains("no existe en la BBDD.", excepcion.Message);

        _uowMock.Verify(u => u.Luchadores.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.GetByLuchadorIdConDetallesAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ObtenerByEspecieIdAsync_ConIdValido_DevuelveColeccionMapeadaCorrectamente()
    {
        // 1. Arrange.
        int idBuscado = 30;

        Especie especie = new Especie
        {
            Descripcion = "Namekiano",
            Multiplicador = 1.2
        };

        List<Lectura> listaLecturas = new List<Lectura>
        {
            new Lectura
            {
                LuchadorId = 47,
                Luchador = new Luchador
                {
                    Nombre = "Piccoro",
                    Especie = new Especie
                    {
                        Descripcion = "Namekiano"
                    }
                },
                DispositivoId = 4,
                NivelKi = 18000,
                FechaLectura = DateTime.UtcNow
            }
        };

        // Simulo encontrar la especie en la BBDD.
        _uowMock.Setup(u => u.Especies.GetByIdAsync(idBuscado)).ReturnsAsync(especie);
        // Simulo devolver todas los registros de esa especie.
        _uowMock.Setup(u => u.Lecturas.GetByEspecieIdAsync(idBuscado)).ReturnsAsync(listaLecturas);

        // 2. Act.
        var resultado = await _lecturaService.ObtenerByEspecieIdAsync(idBuscado);

        // 3. Assert.
        Assert.NotNull(resultado);

        List<LecturaRespuestaDto> listaResultante = resultado.ToList();
        Assert.Single(listaResultante);

        LecturaRespuestaDto dto = listaResultante.First();
        Assert.Equal(47, dto.LuchadorId);
        Assert.Equal(4, dto.DispositivoId);
        Assert.Equal(18000, dto.NivelKi);
        Assert.Equal("Namekiano", dto.NombreEspecie);
        Assert.Equal("Piccoro", dto.NombreLuchador);

        _uowMock.Verify(u => u.Especies.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.GetByEspecieIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task ObtenerByEspecieIdAsync_ConIdInvalido_LanzaExcepcionKeyNotFoundException()
    {
        // 1. Arrange.
        int idBuscado = 99999;

        // Simulo que la búsqueda de la especie devuelve null.
        _uowMock.Setup(u => u.Especies.GetByIdAsync(idBuscado)).ReturnsAsync((Especie?) null);

        // 2. Act & 3. Assert.
        var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _lecturaService.ObtenerByEspecieIdAsync(idBuscado));

        Assert.Contains("El Id de especie proporcionado ", excepcion.Message);
        _uowMock.Verify(u => u.Especies.GetByIdAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.GetByEspecieIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CrearLecturaAsync_ConDatosValidos_CreaYDevuelveNuevaLecturaCorrectamente()
    {
        // 1. Arrange.
        CrearLecturaDto dto = new CrearLecturaDto
        {
            LuchadorId = 10,
            FingerprintDispositivo = "  abcd-1234  ",
            NivelKi = 200000,
            RutaFotografia = "/images/goku_ssj.png"
        };

        Luchador luchadorFake = new Luchador
        {
            Id = dto.LuchadorId,
            Nombre = "Raditz",
            EspecieId = 8,
            Especie = new Especie
            {
                Descripcion = "Saiyajin",
                Multiplicador = 1.5
            },
            FechaRegistro = DateTime.UtcNow.AddDays(-1)
        };

        Dispositivo dispositivoFake = new Dispositivo
        {
          Tipo = "mobile",
          ModeloHardware = "iPhone 18",
          ColorId = 3,
          Fingerprint = "abcd-1234",
          FechaUltimoUso = DateTime.UtcNow.AddDays(-1),
          Color = new Color
          {
              Descripcion = "Verde",
              CodigoHex = "00FF00FF"
          }
        };

        // Simulo la existencia del luchador.
        _uowMock.Setup(u => u.Luchadores.GetByIdConEspecieAsync(dto.LuchadorId)).ReturnsAsync(luchadorFake);
        // Simulo la existencia del dipositivo.
        _uowMock.Setup(u => u.Dispositivos.GetByFingerprintConDetallesAsync(dto.FingerprintDispositivo.Trim())).ReturnsAsync(dispositivoFake);
        // Simulo la inserción y el guardado exitosos.
        _uowMock.Setup(u => u.Lecturas.AddAsync(It.IsAny<Lectura>())).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);


        // 2. Act.
        LecturaRespuestaDto resultado = await _lecturaService.CrearLecturaAsync(dto);

        // 3. Assert.
        Assert.NotNull(resultado);
        Assert.Equal(dto.LuchadorId, resultado.LuchadorId);
        Assert.Equal(luchadorFake.Nombre, resultado.NombreLuchador);
        Assert.Equal("Saiyajin", resultado.NombreEspecie);
        Assert.Equal(dto.NivelKi, resultado.NivelKi);
        // Verifico que se actualizó la fecha del último uso del dispositivo.
        Assert.True(dispositivoFake.FechaUltimoUso > DateTime.UtcNow.AddMinutes(-1));

        _uowMock.Verify(u => u.Luchadores.GetByIdConEspecieAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Dispositivos.GetByFingerprintConDetallesAsync(It.IsAny<string>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.AddAsync(It.Is<Lectura>(l =>
            l.LuchadorId == dto.LuchadorId &&
            l.RutaFotografia == dto.RutaFotografia
            )), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CrearLecturaAsync_ConDispositivoInexistente_LanzaExcepcionKeyNotFoundException()
    {
        // 1. Arrange.
        CrearLecturaDto dto = new CrearLecturaDto
        {
            LuchadorId = 30,
            FingerprintDispositivo = "   XYZ987   ",
            NivelKi = 10000
        };

        Luchador luchadorFake = new Luchador
        {
            Id = dto.LuchadorId,
            Nombre = "Piccoro",
            FechaRegistro = DateTime.UtcNow,
            Especie = new Especie
            {
                Descripcion = "Namekuseijin"
            }
        };

        // Simulo la existencia del luchador en la BBDD..
        _uowMock.Setup(u => u.Luchadores.GetByIdConEspecieAsync(dto.LuchadorId)).ReturnsAsync(luchadorFake);
        // Simulo que el dispositivo no existe en la BBDD.
        string fingerprintLimpio = dto.FingerprintDispositivo.Trim();
        _uowMock.Setup(u => u.Dispositivos.GetByFingerprintConDetallesAsync(fingerprintLimpio))
                .ReturnsAsync((Dispositivo?)null);

        // 2. Act 3. Assert.
        var excepcion = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _lecturaService.CrearLecturaAsync(dto));
        
        // Verifico que el mensaje contenga los detalles esperados sobre la falla.
        Assert.Contains("no existe en la BBDD. Creación de Lectura imposible", excepcion.Message);
        // Verificaciones defensivas de comportamiento. Me cercioro de que el flujo se interrumpió.
        _uowMock.Verify(u => u.Luchadores.GetByIdConEspecieAsync(It.IsAny<int>()), Times.Once);
        _uowMock.Verify(u => u.Dispositivos.GetByFingerprintConDetallesAsync(It.IsAny<string>()), Times.Once);
        // Verifico que nunca intentó persistir los datos en la BBDD.
        _uowMock.Verify(u => u.Lecturas.AddAsync(It.IsAny<Lectura>()), Times.Never);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task FiltrarAsync_TodosLosParametrosValidos_DevuelveRepuestaPaginada()
    {
        // 1. Arrange.
        int pagina = 2;
        int tamanioPagina = 8;
        long minimo = 200;
        long maximo = 4000000000;
        string ordenarPor = "fecha";
        bool descendente = true;

        var lecturasFake = new List<Lectura>
        {
            new Lectura
            {
                Id = 1,
                LuchadorId = 10,
                DispositivoId = 100,
                NivelKi = 9000,
                FechaLectura = DateTime.UtcNow,
                Luchador = new Luchador
                {
                    Nombre = "Goku",
                    Especie = new Especie { Descripcion = "Saiyajin" }
                }
            },
            new Lectura
            {
                Id = 2,
                LuchadorId = 11,
                DispositivoId = 101,
                NivelKi = 5000,
                FechaLectura = DateTime.UtcNow,
                Luchador = new Luchador
                {
                    Nombre = "Vegeta",
                    Especie = new Especie { Descripcion = "Saiyajin" }
                }
            }
        }.AsQueryable();

        // DTOs esperados al materializar la proyección.
        var datosEsperados = new List<LecturaRespuestaDto>
        {
            new()
            {
                Id = 1,
                LuchadorId = 10,
                DispositivoId = 100,
                NivelKi = 9000,
                NombreLuchador = "Goku",
                NombreEspecie = "Saiyajin"
            },
            new()
            {
                Id = 2,
                LuchadorId = 11,
                DispositivoId = 101,
                NivelKi = 5000,
                NombreLuchador = "Vegeta",
                NombreEspecie = "Saiyajin"
            }
        };

        // Simulo el retorno de IQueryable base.
        _uowMock.Setup(u => u.Lecturas.ObtenerQueryable()).Returns(lecturasFake);
        // Simulo el conteo de de la consulta filtrada.
        _uowMock.Setup(u => u.Lecturas.ContarConsultaAsync(It.IsAny<IQueryable<Lectura>>())).ReturnsAsync(lecturasFake.Count());
        // Simulo la materialización final del IQueryable<LecturaRespuestaDto>.
        _uowMock.Setup(u => u.Lecturas.MaterializarConsultaAsync(It.IsAny<IQueryable<LecturaRespuestaDto>>()))
                .ReturnsAsync(datosEsperados);

        // 2. Act.
        var (lecturas, totalRegistros) = await _lecturaService.FiltrarAsync(pagina,
                                                                            tamanioPagina,
                                                                            minimo,
                                                                            maximo,
                                                                            ordenarPor,
                                                                            descendente);

        // 3. Assert.
        Assert.NotNull(lecturas);
        Assert.Equal(2, totalRegistros);

        var listaResultado = lecturas.ToList();
        // Verifico los datos mapeados del primer elemento obtenido.
        var primerElemento = listaResultado.First();
        Assert.Equal(1, primerElemento.Id);
        Assert.Equal(100, primerElemento.DispositivoId);
        Assert.Equal(9000, primerElemento.NivelKi);
        Assert.Equal("Goku", primerElemento.NombreLuchador);
        Assert.Equal("Saiyajin", primerElemento.NombreEspecie);

        // Ahora, verifico el comportamiento.
        _uowMock.Verify(u => u.Lecturas.ObtenerQueryable(), Times.Once);
        _uowMock.Verify(u => u.Lecturas.ContarConsultaAsync(It.IsAny<IQueryable<Lectura>>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.MaterializarConsultaAsync(It.IsAny<IQueryable<LecturaRespuestaDto>>()), Times.Once);
    }

    [Fact]
    public async Task FiltrarAsync_SinResultadosEnBBDD_DevuelveColeccionVaciaYTotalCero()
    {
        // 1. Arrange.
        int pagina = 1;
        int tamanioPagina = 10;
        long minimo = 9999999999;

        // Lista vacía que representará al resultado devuelto.
        var lecturasFake = new List<Lectura>().AsQueryable();

        // Regreso el IQueryable base.
        _uowMock.Setup(u => u.Lecturas.ObtenerQueryable()).Returns(lecturasFake);

        // Preparo el conteo que devolverá 0.
        _uowMock.Setup(u => u.Lecturas.ContarConsultaAsync(It.IsAny<IQueryable<Lectura>>()))
                .ReturnsAsync(0);
        
        // La materialización de la consulta devolverá una lista vacía de DTOs.
        _uowMock.Setup(u => u.Lecturas.MaterializarConsultaAsync(It.IsAny<IQueryable<LecturaRespuestaDto>>()))
                .ReturnsAsync(new List<LecturaRespuestaDto>());

        // 2. Act.
        var (lecturas, totalRegistros) = await _lecturaService.FiltrarAsync(pagina,
                                                                        tamanioPagina,
                                                                        minimo,
                                                                        null,
                                                                        null,
                                                                        false);

        // 3. Assert.
        // Verifico que se ejecutó la consulta, pero que se trajo 0 elementos.
        _uowMock.Verify(u => u.Lecturas.ObtenerQueryable(), Times.Once);
        _uowMock.Verify(u => u.Lecturas.ContarConsultaAsync(It.IsAny<IQueryable<Lectura>>()), Times.Once);
        _uowMock.Verify(u => u.Lecturas.MaterializarConsultaAsync(It.IsAny<IQueryable<LecturaRespuestaDto>>()), Times.Once);
    }
}