using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using KiTrackerApi.Services;

namespace KiTrackerApi.Tests.Services;

public class JwtTokenServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;

    public JwtTokenServiceTests()
    {
        // Se ejecuta antes de cada prueba (garantiza aislamiento)
        _configurationMock = new Mock<IConfiguration>();
    }

    [Fact]
    public void GenerarToken_ConConfiguracionValida_DevuelveTokenYFechaExpiracion()
    {
        // 1. Arrange.
        // Simulamos las claves requeridas por el método en IConfiguration
        _configurationMock.Setup(c => c["Jwt:Clave"]).Returns("ClaveSuperSecretaDeAlMenos32CaracteresLarga!");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("KiTrackerIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("KiTrackerAudience");

        // Simulamos la sección de expiración (IConfigurationSection)
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s.Value).Returns("60"); // 60 minutos, como texto.
        _configurationMock.Setup(c => c.GetSection("Jwt:ExpiracionEnMinutos")).Returns(mockSection.Object);

        var service = new JwtTokenService(_configurationMock.Object);
        var usuario = "goku";
        var email = "goku@ki.com";

        // 2. Act.
        var resultado = service.GenerarToken(usuario, email);

        // 3. Assert.
        Assert.NotNull(resultado.Token);
        Assert.NotEmpty(resultado.Token);
        Assert.True(resultado.Expiracion > DateTime.UtcNow);

        // Opcional: Deserializar el JWT para verificar que contenga los claims esperados
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(resultado.Token);

        Assert.Equal("KiTrackerIssuer", jwtToken.Issuer);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == usuario);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == email);
    }

    [Fact]
    public void GenerarToken_SinClaveConfigurada_LanzaInvalidOperationException()
    {
        // 1. Arrange.
        // Simulamos que la configuración devuelve null para "Jwt:Clave".
        _configurationMock.Setup(c => c["Jwt:Clave"]).Returns((string?)null);

        string usuario = "goku";
        string correo = "goku@ki.com";
        var service = new JwtTokenService(_configurationMock.Object);

        // 2. Act & 3. Assert.
        // Verificamos que se lance la excepción esperada al invocar el método.
        var excepcion = Assert.Throws<InvalidOperationException>(() => 
            service.GenerarToken(usuario, correo)
        );

        Assert.Contains("Falta la clave de firma del JWT", excepcion.Message);
    }

    [Theory]
    [InlineData("Q2xhdmVTdWdlcmlkYUNvblN1ZmljaWVudGVMb25naXR1ZDEyMzQ1Ng==")] // Clave en Base64
    [InlineData("ClaveSuperSecretaDeAlMenos32CaracteresLarga!")]            // Clave en UTF8 plano
    public void GenerarToken_ConDiferentesFormatosDeClave_GeneraTokenCorrectamente(string claveConfigurada)
    {
        // 1. Arrange.
        _configurationMock.Setup(c => c["Jwt:Clave"]).Returns(claveConfigurada);

        Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(s => s.Value).Returns("60");
        _configurationMock.Setup(c => c.GetSection("Jwt:ExpiracionEnMinutos")).Returns(mockSection.Object);

        string usuario = "vegeta";
        string correo = "vegeta@ki.com";
        var service = new JwtTokenService(_configurationMock.Object);

        // 2. Act.
        var resultado = service.GenerarToken(usuario, correo);

        // 3. Assert.
        Assert.NotNull(resultado.Token);
        Assert.NotEmpty(resultado.Token);
    }
}