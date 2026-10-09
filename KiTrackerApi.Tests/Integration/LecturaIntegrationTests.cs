using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using KiTrackerApi.Core.Features.Lecturas.DTOs;
using KiTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Xunit;

namespace KiTrackerApi.Tests.Integration;

public class LecturaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public LecturaIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Levanta el servidor en memoria (TestServer) y crea un cliente HTTP.
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetLecturasPorRangoDeKiAsync_ConUsuarioAutenticadoYRangoValido_Devuelve200OkYColeccionFiltrada()
    {
        // 1. Arrange.
        long minimo = 1000;
        long maximo = 10000;

        // Genero un token JWT válido haciendo uso del servicio real de tokens desde el contenedor IoC
        // de la factoría.
        string tokenValido = GenerarTokenDePrueba();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValido);

        // 2. Act.
        var respuesta = await _httpClient.GetAsync($"/lecturas/rango?minimo={minimo}&maximo={maximo}");

        // 3. Assert.
        // Verificación del código de estado HTTP 200 OK.
        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        // Deserealizo y verifico la colección de DTOs devuelta.
        var resultado = await respuesta.Content.ReadFromJsonAsync<IEnumerable<LecturaRespuestaDto>>();

        Assert.NotNull(resultado);
        // Verifico que todas las lecturas regresadas cumplan estríctamente con el rango de ki solicitado.
        Assert.All(resultado, lectura =>
        {
            Assert.True(lectura.NivelKi >= minimo && lectura.NivelKi <= maximo);
        });
    }

    // [Fact]
    // public async Task GetLecturasPorRangoDeKi_SinAutenticacion_Devuelve401Unauthorized()
    // {
    //     // 1. Arrange.
    //     int minimo = 1000;
    //     int maximo = 5000;
    //     _httpClient.DefaultRequestHeaders.Authorization = null;

    //     // 2. Act.
    //     var respuesta = await _httpClient.GetAsync($"/lecturas/rango?minimo={minimo}&maximo={maximo}");

    //     // 3. Assert.
    //     Assert.Equal(HttpStatusCode.Unauthorized, respuesta.StatusCode);
    // }

    [Fact]
    public async Task GetLecturasPorRangoDeKi_ConMinimoMayorQueMaximo_Devuelve400BadRequest()
    {
        // 1. Arrange.
        string tokenValido = GenerarTokenDePrueba();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValido);

        long minimoInvalido = 50000;
        long maximoInvalido = 1000;

        // 2. Act.
        var respuesta = await _httpClient.GetAsync($"/lecturas/rango?minimo={minimoInvalido}&maximo={maximoInvalido}");

        // 3. Assert.
        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    // Métodos privados de utilería.
    private string GenerarTokenDePrueba()
    {
        using var scope = _factory.Services.CreateScope();

        // Obtengo el servicio real de JWT de esta aplicación (por ejemplo, IJwtTokenService).
        var jwtService = scope.ServiceProvider.GetRequiredService<JwtTokenService>();

        // Genero un JWT válido para un usuario de prueba de KiTracker.
        var (token, expiracion) = jwtService.GenerarToken("UsuarioTest1", "usuario.test@example.net");
        return token;
    }
}