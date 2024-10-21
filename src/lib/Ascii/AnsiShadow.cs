// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using System.Text;

namespace Ascii;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AnsiShadow {
    private readonly StringBuilder _sb = new();
    private readonly string _r = AnsiCodes.ResetGraphicsModes;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    public string Convert(string text, string foreground = "white", string shadow = "white") {
        _sb.Clear();

        string f = AnsiColor.AsFore(foreground.ToLowerInvariant());
        string b = AnsiColor.AsFore(shadow.ToLowerInvariant());
        
        
        List<string[]> textStrings = [];
        textStrings.AddRange(text
            .ToLowerInvariant()
            .Select(character => character switch {
                    'a' => LetterA(f, b),
                    'b' => LetterB(f, b),
                    'c' => LetterC(f, b),
                    'd' => LetterD(f, b),
                    'e' => LetterE(f, b),
                    'f' => LetterF(f, b),
                    'g' => LetterG(f, b),
                    'h' => LetterH(f, b),
                    'i' => LetterI(f, b),
                    'j' => LetterJ(f, b),
                    'k' => LetterK(f, b),
                    'l' => LetterL(f, b),
                    'm' => LetterM(f, b),
                    'n' => LetterN(f, b),
                    'o' => LetterO(f, b),
                    'p' => LetterP(f, b),
                    'q' => LetterQ(f, b),
                    'r' => LetterR(f, b),
                    's' => LetterS(f, b),
                    't' => LetterT(f, b),
                    'u' => LetterU(f, b),
                    'v' => LetterV(f, b),
                    'w' => LetterW(f, b),
                    'x' => LetterX(f, b),
                    'y' => LetterY(f, b),
                    'z' => LetterZ(f, b),

                    // Any undefined character is a space
                    _ => LetterSpace()
                }
            ));

        int lines = textStrings.Max(t => t.Length);
        for (int i = 0; i < lines; i++) {
            foreach (string[] textString in textStrings) {
                _sb.Append(textString[i]);
                _sb.Append(_r);
            }

            _sb.AppendLine();
        }

        return _sb.ToString();
    }

    public static string[] LetterA(string f, string b) => [
        $" {f}█████{b}╗ ",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}███████{b}║",
        $"{f}██{b}╔══{f}██{b}║",
        $"{f}██{b}║  {f}██{b}║",
        $"{b}╚═╝  ╚═╝"
    ];

    public static string[] LetterB(string f, string b) => [
        $"{f}██████{b}╗ ",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██████{b}╔╝",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██████{b}╔╝",
        $"{b}╚═════╝ "
    ];

    public static string[] LetterC(string f, string b) => [
        $" {f}██████{b}╗",
        $"{f}██{b}╔════╝",
        $"{f}██{b}║     ",
        $"{f}██{b}║     ",
        $"{b}╚{f}██████{b}╗",
        $" {b}╚═════╝"
    ];

    public static string[] LetterD(string f, string b) => [
        $"{f}██████{b}╗ ",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██{b}║  {f}██{b}║",
        $"{f}██{b}║  {f}██{b}║",
        $"{f}██████{b}╔╝",
        $"{b}╚═════╝ "
    ];

    public static string[] LetterE(string f, string b) => [
        $"{f}███████{b}╗",
        $"{f}██{b}╔════╝",
        $"{f}█████{b}╗  ",
        $"{f}██{b}╔══╝  ",
        $"{f}███████{b}╗",
        $"{b}╚══════╝"
    ];

    public static string[] LetterF(string f, string b) => [
        $"{f}███████{b}╗",
        $"{f}██{b}╔════╝",
        $"{f}█████{b}╗  ",
        $"{f}██{b}╔══╝  ",
        $"{f}██{b}║     ",
        $"{b}╚═╝     "
    ];

    public static string[] LetterG(string f, string b) => [
        $" {f}██████{b}╗ ",
        $"{f}██{b}╔════╝ ",
        $"{f}██{b}║  {f}███{b}╗",
        $"{f}██{b}║   {f}██{b}║",
        $"{b}╚{f}██████{b}╔╝",
        $" {b}╚═════╝ "
    ];

    public static string[] LetterH(string f, string b) => [
        $"{f}██{b}╗  {f}██{b}╗",
        $"{f}██{b}║  {f}██{b}║",
        $"{f}███████{b}║",
        $"{f}██{b}╔══{f}██{b}║",
        $"{f}██{b}║  {f}██{b}║",
        $"{b}╚═╝  {b}╚═╝"
    ];

    public static string[] LetterI(string f, string b) => [
        $"{f}██{b}╗",
        $"{f}██{b}║",
        $"{f}██{b}║",
        $"{f}██{b}║",
        $"{f}██{b}║",
        $"{b}╚═╝"
    ];

    public static string[] LetterJ(string f, string b) => [
        $"     {f}██{b}╗",
        $"     {f}██{b}║",
        $"     {f}██{b}║",
        $"{f}██   {f}██{b}║",
        $"{b}╚{f}█████{b}╔╝",
        $" {b}╚════╝ "
    ];

    public static string[] LetterK(string f, string b) => [
        $"{f}██{b}╗  {f}██{b}╗",
        $"{f}██{b}║ {f}██{b}╔╝",
        $"{f}█████{b}╔╝ ",
        $"{f}██{b}╔═{f}██{b}╗ ",
        $"{f}██{b}║  {f}██{b}╗",
        $"{b}╚═╝  {b}╚═╝"
    ];

    public static string[] LetterL(string f, string b) => [
        $"{f}██{b}╗     ",
        $"{f}██{b}║     ",
        $"{f}██{b}║     ",
        $"{f}██{b}║     ",
        $"{f}███████{b}╗",
        $"{b}╚══════╝"
    ];

    public static string[] LetterM(string f, string b) => [
        $"{f}███{b}╗   {f}███{b}╗",
        $"{f}████{b}╗ {f}████{b}║",
        $"{f}██{b}╔{f}████{b}╔{f}██{b}║",
        $"{f}██{b}║{b}╚{f}██{b}╔╝{f}██{b}║",
        $"{f}██{b}║ {b}╚═╝ {f}██{b}║",
        $"{b}╚═╝     {b}╚═╝"
    ];

    public static string[] LetterN(string f, string b) => [
        $"{f}███{b}╗   {f}██{b}╗",
        $"{f}████{b}╗  {f}██{b}║",
        $"{f}██{b}╔{f}██{b}╗ {f}██{b}║",
        $"{f}██{b}║{b}╚{f}██{b}╗{f}██{b}║",
        $"{f}██{b}║ {b}╚{f}████{b}║",
        $"{b}╚═╝  {b}╚═══╝"
    ];

    public static string[] LetterO(string f, string b) => [
        $" {f}██████{b}╗ ",
        $"{f}██{b}╔═══{f}██{b}╗",
        $"{f}██{b}║   {f}██{b}║",
        $"{f}██{b}║   {f}██{b}║",
        $"{b}╚{f}██████{b}╔╝",
        $" {b}╚═════╝ "
    ];

    public static string[] LetterP(string f, string b) => [
        $"{f}██████{b}╗ ",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██████{b}╔╝",
        $"{f}██{b}╔═══╝ ",
        $"{f}██{b}║     ",
        $"{b}╚═╝     "
    ];

    public static string[] LetterQ(string f, string b) => [
        $" {f}██████{b}╗ ",
        $"{f}██{b}╔═══{f}██{b}╗",
        $"{f}██{b}║   {f}██{b}║",
        $"{f}██{b}║{f}▄▄ ██{b}║",
        $"{b}╚{f}██████{b}╔╝",
        $" {b}╚══{f}▀▀{b}═╝ "
    ];

    public static string[] LetterR(string f, string b) => [
        $"{f}██████{b}╗ ",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██████{b}╔╝",
        $"{f}██{b}╔══{f}██{b}╗",
        $"{f}██{b}║  {f}██{b}║",
        $"{b}╚═╝  {b}╚═╝"
    ];

    public static string[] LetterS(string f, string b) => [
        $"{f}███████{b}╗",
        $"{f}██{b}╔════╝",
        $"{f}███████{b}╗",
        $"{b}╚════{f}██{b}║",
        $"{f}███████{b}║",
        $"{b}╚══════╝"
    ];

    public static string[] LetterT(string f, string b) => [
        $"{f}████████{b}╗",
        $"{b}╚══{f}██{b}╔══╝",
        $"   {f}██{b}║   ",
        $"   {f}██{b}║   ",
        $"   {f}██{b}║   ",
        $"   {b}╚═╝   "
    ];

    public static string[] LetterU(string f, string b) => [
        $"{f}██{b}╗   {f}██{b}╗",
        $"{f}██{b}║   {f}██{b}║",
        $"{f}██{b}║   {f}██{b}║",
        $"{f}██{b}║   {f}██{b}║",
        $"{b}╚{f}██████{b}╔╝",
        $" {b}╚═════╝ "
    ];

    public static string[] LetterV(string f, string b) => [
        $"{f}██{b}╗   {f}██{b}╗",
        $"{f}██{b}║   {f}██{b}║",
        $"{f}██{b}║   {f}██{b}║",
        $"{b}╚{f}██{b}╗ {f}██{b}╔╝",
        $" {b}╚{f}████{b}╔╝ ",
        $"  {b}╚═══╝  "
    ];

    public static string[] LetterW(string f, string b) => [
        $"{f}██{b}╗    {f}██{b}╗",
        $"{f}██{b}║    {f}██{b}║",
        $"{f}██{b}║ {f}█{b}╗ {f}██{b}║",
        $"{f}██{b}║{f}███{b}╗{f}██{b}║",
        $"{b}╚{f}███{b}╔{f}███{b}╔╝",
        $" {b}╚══╝{b}╚══╝ "
    ];

    public static string[] LetterX(string f, string b) => [
        $"{f}██{b}╗  {f}██{b}╗",
        $"{b}╚{f}██{b}╗{f}██{b}╔╝",
        $" {b}╚{f}███{b}╔╝ ",
        $" {f}██{b}╔{f}██{b}╗ ",
        $"{f}██{b}╔╝ {f}██{b}╗",
        $"{b}╚═╝  {b}╚═╝"
    ];

    public static string[] LetterY(string f, string b) => [
        $"{f}██{b}╗   {f}██{b}╗",
        $"{b}╚{f}██{b}╗ {f}██{b}╔╝",
        $" {b}╚{f}████{b}╔╝ ",
        $"  {b}╚{f}██{b}╔╝  ",
        $"   {f}██{b}║   ",
        $"   {b}╚═╝   "
    ];

    public static string[] LetterZ(string f, string b) => [
        $"{f}███████{b}╗",
        $"{b}╚══{f}███{b}╔╝",
        $"  {f}███{b}╔╝ ",
        $" {f}███{b}╔╝  ",
        $"{f}███████{b}╗",
        $"{b}╚══════╝"
    ];

    public static string[] LetterSpace() => [
        $"     ",
        $"     ",
        $"     ",
        $"     ",
        $"     ",
        $"     "
    ];

}
