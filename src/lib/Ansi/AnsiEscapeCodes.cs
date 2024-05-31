// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     A utility class containing ANSI escape codes for terminal control.
/// </summary>
public static class AnsiEscapeCodes {
    public const string EscapeCtrl = "^[";
    public const string EscapeOctal = "\033";
    public const string EscapeUnicode = "\u001b";
    public const string EscapeHexadecimal = "\x1B";
}
