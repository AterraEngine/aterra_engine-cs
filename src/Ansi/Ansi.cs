// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AnsiColor {
    // -----------------------------------------------------------------------------------------------------------------
    // Logic
    // -----------------------------------------------------------------------------------------------------------------
    private static ByteVector3 _tryGetColor(string colorName) {
        return !AnsiColors.KnownColorsDictionary.TryGetValue(colorName, out var value)
            ? ByteVector3.Max
            : value;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // String Logic
    // -----------------------------------------------------------------------------------------------------------------
    
    public static string F(string colorName, string? text) => Fore(colorName, text);
    public static string Fore(string colorName, string? text) => $"{AnsiCodes.RgbForegroundColor(_tryGetColor(colorName))}{text}{AnsiCodes.ResetGraphicsModes}";
    
    public static string B(string colorName, string? text) => Back(colorName, text);
    public static string Back(string colorName, string? text) => $"{AnsiCodes.RgbBackgroundColor(_tryGetColor(colorName))}{text}{AnsiCodes.ResetGraphicsModes}";
    
    public static string U(string colorName, string? text) => Under(colorName, text);
    public static string Under(string colorName, string? text) => $"{AnsiCodes.RgbUnderlineColor(_tryGetColor(colorName))}{text}{AnsiCodes.ResetGraphicsModes}";
}