// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Text;

namespace Ascii;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AsciiText<TConfig>(Action<TConfig>? configure, int stringBuilderCapacity = 1000)
    where TConfig : AsciiTextConfig, new() {
    private readonly StringBuilder _sb = new(stringBuilderCapacity);
    private string _lastConvertedWord = string.Empty;
    private TConfig _config = RegisterConfig(configure);
    private readonly Dictionary<char, string[]> _charactersWithGraphics = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    private void Clear() {
        _sb.Clear();
        _lastConvertedWord = string.Empty;
        _charactersWithGraphics.Clear();
    }

    private static TConfig RegisterConfig(Action<TConfig>? configure, TConfig? config = null) {
        TConfig configInstance = config ?? new TConfig();
        configure?.Invoke(configInstance);

        if (configInstance.AlphabetDictionary.ContainsKey(' ')) return configInstance;

        int lineCount = configInstance.AlphabetDictionary.Max(x => x.Value.Max(line => line.Length));
        string[] space = new string[lineCount];
        string spaceChars = new(' ', configInstance.SpaceCharWidth);

        for (int i = 0; i < lineCount; i++) {
            space[i] = spaceChars;
        }

        configInstance.AlphabetDictionary[' '] = space.ToImmutableArray();

        return configInstance;
    }

    public void UpdateConfig(Action<TConfig> configure) {
        _config = RegisterConfig(configure, _config);
        Clear();
    }

    private string[] GetWithGraphics(char c) {
        if (!_config.CapitalizationSensitive) c = char.ToLowerInvariant(c);

        if (_charactersWithGraphics.TryGetValue(c, out string[]? letterArray)) return letterArray;
        if (!_config.AlphabetDictionary.TryGetValue(c, out ImmutableArray<string> stringArray)) return _config.AlphabetDictionary[' '].ToArray();

        string[] localArray = new string[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++) {
            var localBuilder = new StringBuilder(stringArray[i].Length);
            foreach (char c1 in stringArray[i]) {
                localBuilder.Append(_config.GraphicsDictionary.TryGetValue(c1, out string? graphics) ? graphics : c1);
            }

            localArray[i] = localBuilder.ToString();
        }

        _charactersWithGraphics[c] = localArray;
        return localArray;
    }

    private ImmutableArray<string> GetWithoutGraphics(char c) {
        if (!_config.CapitalizationSensitive) c = char.ToLowerInvariant(c);
        return _config.AlphabetDictionary.TryGetValue(c, out ImmutableArray<string> letterArray) ? letterArray : _config.AlphabetDictionary[' '];
    }

    public string ConvertToAsciiString(string text) {
        if (_config.Border != null) return WithBorder(text);

        return text == _lastConvertedWord ? _sb.ToString() : NormalOutput(text);
    }

    private string NormalOutput(string text) {
        _sb.Clear();

        string[][] textArray = text.Select(GetWithGraphics).ToArray();
        int lines = textArray.Max(t => t.Length);

        for (int i = 0; i < lines; i++) {

            ExtractAsciiText(textArray, i);

            _sb.AppendLine();
        }

        _lastConvertedWord = text;
        return _sb.ToString();
    }

    private string WithBorder(string text) {
        IReadOnlyDictionary<BorderAssignment, ImmutableArray<string>> borderGraphics = _config.Border!.BorderGraphics;
        int chunkSize = borderGraphics[BorderAssignment.CornerTopLeft][0].Length;// Assuming chunk sizes are consistent

        ImmutableArray<string>[] colorLessTextArray = text.Select(GetWithoutGraphics).ToArray();
        string[][] textArray = text.Select(GetWithGraphics).ToArray();

        int totalLineLength = colorLessTextArray.Sum(lines => lines[0].Length);
        int remainderWidth = totalLineLength % chunkSize;
        if (remainderWidth != 0) totalLineLength += chunkSize - remainderWidth;
        int straightSteps = totalLineLength / chunkSize;

        int textHeight = textArray.Max(arr => arr.Length);
        int remainderHeight = textHeight % chunkSize;
        if (remainderHeight != 0) textHeight += chunkSize - remainderHeight;

        _sb.Clear();

        // Top border
        GenerateAsciiBorder(
            chunkSize,
            borderGraphics[BorderAssignment.CornerTopLeft],
            straightSteps,
            borderGraphics[BorderAssignment.StraightTop],
            borderGraphics[BorderAssignment.CornerTopRight]
        );

        // Text with side borders
        for (int i = 0; i < textHeight; i++) {
            _sb.Append(borderGraphics[BorderAssignment.StraightLeft][i % chunkSize]);

            ExtractAsciiText(textArray, i);

            _sb.AppendLine(borderGraphics[BorderAssignment.StraightRight][i % chunkSize]);
        }

        // Bottom border
        GenerateAsciiBorder(
            chunkSize,
            borderGraphics[BorderAssignment.CornerBottomLeft],
            straightSteps,
            borderGraphics[BorderAssignment.StraightBottom],
            borderGraphics[BorderAssignment.CornerBottomRight]
        );

        _lastConvertedWord = text;
        return _sb.ToString();
    }

    private void ExtractAsciiText(string[][] textArray, int i) {
        foreach (string[] textString in textArray) {
            _sb.Append(i < textString.Length
                ? textString[i]
                : new string(' ', textString[0].Length));
        }
    }

    private void GenerateAsciiBorder(int chunkSize, ImmutableArray<string> cornerLeft, int straightSteps, ImmutableArray<string> straight, ImmutableArray<string> cornerRight) {
        for (int i = 0; i < chunkSize; i++) {
            _sb.Append(cornerLeft[i]);

            for (int j = 0; j < straightSteps; j++) {
                _sb.Append(straight[i % chunkSize]);
            }

            _sb.AppendLine(cornerRight[i]);
        }
    }
}
