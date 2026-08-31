using System.Globalization;

namespace KiTrackerApi.Core.Extensions;

public static class StringExtensions
{
    public static string SanitizarNombrePropio(this string? nombre)
    {
        if(string.IsNullOrWhiteSpace(nombre))
            return string.Empty;

        // 1. Sanitización del texto.
        string limpio = nombre.Trim();

        // 2. Convertir a Title Case.
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(limpio.ToLower());
    }

    public static string SanitizarTextoLargo(this string? texto)
    {
        if(string.IsNullOrEmpty(texto))
            return string.Empty;

        // 1. Retiro los espacios en los extremos.
        var limpio = texto.Trim();

        // 2. Me aseguro de que la primera letra del texto sea mayúscula (formato de oración).
        if(char.IsLower(limpio[0]))
            limpio = string.Concat(char.ToUpper(limpio[0], CultureInfo.CurrentCulture), limpio.Substring(1));

        return limpio;
    }
}