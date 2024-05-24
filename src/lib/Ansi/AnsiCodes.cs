// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// The following class is created by using the following resources:
//      - https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#ansi-escape-sequences
//      - https://github.com/code-of-chaos/AthenaColor (my own package)
/// <summary>
///     Provides ANSI escape codes for cursor control, erase functions, graphics modes, color codes, screen modes, and
///     keyboard strings.
/// </summary>
public static class AnsiCodes {
    private static string _escapeChar = AnsiEscapeCodes.EscapeHexadecimal;

    // -----------------------------------------------------------------------------------------------------------------
    // Cursor Controls
    // -----------------------------------------------------------------------------------------------------------------
    public static string CursorHome => $"{_escapeChar}[H";
    public static string CursorPosition => $"{_escapeChar}[6n";
    public static string CursorMoveUp => $"{_escapeChar} M";
    public static string CursorPositionSaveDec => $"{_escapeChar} 7";
    public static string CursorPositionRestoreDec => $"{_escapeChar} 8";
    public static string CursorPositionSaveSco => $"{_escapeChar}[s";
    public static string CursorPositionRestoreSco => $"{_escapeChar}[u";

    // -----------------------------------------------------------------------------------------------------------------
    // Erase Functions
    // -----------------------------------------------------------------------------------------------------------------
    public static string EraseInDisplay => $"{_escapeChar}[J";
    public static string EraseFromCursorToScreenEnd => $"{_escapeChar}[0J";
    public static string EraseFromCursorToScreenBeginning => $"{_escapeChar}[1J";
    public static string EraseEntireScreen => $"{_escapeChar}[2J";
    public static string EraseSavedLines => $"{_escapeChar}[3J";
    public static string EraseInLine => $"{_escapeChar}[K";
    public static string EraseFromCursorToLineEnd => $"{_escapeChar}[0K";
    public static string EraseFromLineBeginningToCursor => $"{_escapeChar}[1K";
    public static string EraseEntireLine => $"{_escapeChar}[2K";
    public static string ResetGraphicsModes => $"{_escapeChar}[0m";
    public static string SetBoldMode => $"{_escapeChar}[1m";
    public static string ResetBoldMode => $"{_escapeChar}[22m";
    public static string SetDimMode => $"{_escapeChar}[2m";
    public static string ResetDimMode => $"{_escapeChar}[22m";
    public static string SetItalicMode => $"{_escapeChar}[3m";
    public static string ResetItalicMode => $"{_escapeChar}[23m";
    public static string SetUnderlineMode => $"{_escapeChar}[4m";
    public static string ResetUnderlineMode => $"{_escapeChar}[24m";
    public static string SetBlinkingMode => $"{_escapeChar}[5m";
    public static string ResetBlinkingMode => $"{_escapeChar}[25m";
    public static string SetInverseMode => $"{_escapeChar}[7m";
    public static string ResetInverseMode => $"{_escapeChar}[27m";
    public static string SetHiddenMode => $"{_escapeChar}[8m";
    public static string ResetHiddenMode => $"{_escapeChar}[28m";
    public static string SetStrikethroughMode => $"{_escapeChar}[9m";
    public static string ResetStrikethroughMode => $"{_escapeChar}[29m";

    public static string CommonForegroundBlack => $"{_escapeChar}[30m";
    public static string CommonForegroundRed => $"{_escapeChar}[31m";
    public static string CommonForegroundGreen => $"{_escapeChar}[32m";
    public static string CommonForegroundYellow => $"{_escapeChar}[33m";
    public static string CommonForegroundBlue => $"{_escapeChar}[34m";
    public static string CommonForegroundMagenta => $"{_escapeChar}[35m";
    public static string CommonForegroundCyan => $"{_escapeChar}[36m";
    public static string CommonForegroundWhite => $"{_escapeChar}[37m";
    public static string CommonForegroundDefault => $"{_escapeChar}[39m";

    public static string CommonBackgroundBlack => $"{_escapeChar}[40m";
    public static string CommonBackgroundRed => $"{_escapeChar}[41m";
    public static string CommonBackgroundGreen => $"{_escapeChar}[42m";
    public static string CommonBackgroundYellow => $"{_escapeChar}[43m";
    public static string CommonBackgroundBlue => $"{_escapeChar}[44m";
    public static string CommonBackgroundMagenta => $"{_escapeChar}[45m";
    public static string CommonBackgroundCyan => $"{_escapeChar}[46m";
    public static string CommonBackgroundWhite => $"{_escapeChar}[47m";
    public static string CommonBackgroundDefault => $"{_escapeChar}[49m";

    // -----------------------------------------------------------------------------------------------------------------
    // Common private modes
    // -----------------------------------------------------------------------------------------------------------------
    public static string CursorInvisible => $"{_escapeChar}[?25l";
    public static string CursorVisible => $"{_escapeChar}[?25h";
    public static string RestoreScreen => $"{_escapeChar}[?47l";
    public static string SaveScreen => $"{_escapeChar}[?47h";
    public static string EnableAlternativeBuffer => $"{_escapeChar}[?1049h";
    public static string DisableAlternativeBuffer => $"{_escapeChar}[?1049l";
    public static void ChangeEscapeChar(string escape) => _escapeChar = escape;
    public static string CursorMoveH(int line, int column) => $"{_escapeChar}[{line};{column}H";
    public static string CursorMoveF(int line, int column) => $"{_escapeChar}[{line};{column}f";
    public static string CursorMoveUpLines(int lines) => $"{_escapeChar}[{lines}A";
    public static string CursorMoveDownLines(int lines) => $"{_escapeChar}[{lines}B";
    public static string CursorMoveRightColumns(int columns) => $"{_escapeChar}[{columns}C";
    public static string CursorMoveLeftColumns(int columns) => $"{_escapeChar}[{columns}D";
    public static string CursorMoveBeginningNextLine(int lines) => $"{_escapeChar}[{lines}E";
    public static string CursorMoveBeginningPreviousLine(int lines) => $"{_escapeChar}[{lines}F";
    public static string CursorMoveColumn(int columns) => $"{_escapeChar}[{columns}G";

    // -----------------------------------------------------------------------------------------------------------------
    // Colors / Graphics Mode
    // -----------------------------------------------------------------------------------------------------------------
    public static string SetGraphicsModes(params int[] codes) => $"{_escapeChar}[{string.Join(";", codes)}m";

    // -----------------------------------------------------------------------------------------------------------------
    // Color codes
    // -----------------------------------------------------------------------------------------------------------------
    public static string ByteForegroundColor(byte color) => $"{_escapeChar}[38;5;{(int)color}m";
    public static string ByteBackgroundColor(byte color) => $"{_escapeChar}[48;5;{(int)color}m";
    public static string ByteUnderlineColor(byte color) => $"{_escapeChar}[58;5;{(int)color}m";

    public static string RgbForegroundColor(ByteVector3 color) => $"{_escapeChar}[38;2;{color.ToAnsiString()}m";
    public static string RgbBackgroundColor(ByteVector3 color) => $"{_escapeChar}[48;2;{color.ToAnsiString()}m";
    public static string RgbUnderlineColor(ByteVector3 color) => $"{_escapeChar}[58;2;{color.ToAnsiString()}m";

    // -----------------------------------------------------------------------------------------------------------------
    // Screen Modes
    // -----------------------------------------------------------------------------------------------------------------
    public static string SetScreenMode(int value) => $"{_escapeChar}[={value}h";
    public static string ResetScreenMode(int value) => $"{_escapeChar}[={value}l";

    // -----------------------------------------------------------------------------------------------------------------
    // Keyboard Strings
    // -----------------------------------------------------------------------------------------------------------------
    public static string RedefineKeyToCode(string code, string str) => $"{_escapeChar}[{code};{str}p";
}