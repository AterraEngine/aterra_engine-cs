// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Ascii;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AsciiTextConfig {
    public bool UseBorder => Border != null;
    public int SpaceCharWidth = 3; 
    public bool CapitalizationSensitive = false;

    public virtual Dictionary<char, ImmutableArray<string>> AlphabetDictionary { get; } = new();
    public virtual Dictionary<char, string> GraphicsDictionary { get; }  = new();
    
    [MemberNotNullWhen(true, nameof(UseBorder))]
    public virtual AsciiBorder? Border { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RegisterAnsiMarkup(string ansiMarkup, params char[] chars) {
        foreach (char c in chars) {
            GraphicsDictionary.Add(c, $"{ansiMarkup}{c}{AnsiCodes.ResetGraphicsModes}");
        }
    }
    
    public void RegisterLetter(char letter, string[] asciiArt) => AlphabetDictionary.Add(letter, [..asciiArt]);
    
    public void RegisterBorder<T>() where T : AsciiBorder, new() => Border = new T();
    public void RegisterBorder<T>(T border) where T : AsciiBorder, new() => Border = border;
    public void RegisterBorder<T>(Action<T> config) where T : AsciiBorder, new() {
        var border = new T();
        config(border);
        Border = border;
    }
}