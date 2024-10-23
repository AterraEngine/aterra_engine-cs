// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Text;

namespace Ascii;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AsciiText<TConfig>(Action<TConfig>? config) where TConfig : AsciiTextConfig, new() {
    private readonly StringBuilder _sb = new();
    private string _lastConvertedWord = string.Empty;
    private bool _lastConvertedWithBorder;
    private TConfig _config = RegisterConfig(config);
    private readonly Dictionary<char, string[]> _charactersWithGraphics = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void Clear() {
        _lastConvertedWord = string.Empty;
        _sb.Clear();
        _charactersWithGraphics.Clear();
    }

    private static TConfig RegisterConfig(Action<TConfig>? configure, TConfig? config = null) {
        TConfig configInstance = config ?? new TConfig();
        configure?.Invoke(configInstance);

        if (configInstance.AlphabetDictionary.ContainsKey(' ')) return configInstance;

        int lineCount = configInstance.AlphabetDictionary.Max(x => x.Value.Max(line => line.Length));
        string[] space = new string[lineCount];
        string spaceChars = new(' ', configInstance.SpaceCharWidth);
        for (int i = lineCount - 1; i >= 0; i--) {
            space[i] = spaceChars;
        }

        configInstance.AlphabetDictionary.Add(' ', [..space]);

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

        StringBuilder localBuilder = new();
        string[] localArray = new string[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++) {
            char[] charArray = stringArray[i].ToCharArray();

            foreach (char c1 in charArray) {
                if (!_config.GraphicsDictionary.TryGetValue(c1, out string? graphics)) {
                    localBuilder.Append(c1);
                    continue;
                }

                localBuilder.Append(graphics);
            }

            localArray[i] = localBuilder.ToString();
            localBuilder.Clear();
        }

        _charactersWithGraphics.Add(c, localArray);
        return localArray;
    }
    
    private ImmutableArray<string> GetWithoutGraphics(char c) {
        if (!_config.CapitalizationSensitive) c = char.ToLowerInvariant(c);

        return _config.AlphabetDictionary.TryGetValue(c, out ImmutableArray<string> letterArray)
            ? letterArray
            : _config.AlphabetDictionary[' '];
    }
    

    public string ConvertToAsciiString(string text) {
        if (_config.Border is not null) return ConvertToAsciiStringWithBorder(text);
        if (text == _lastConvertedWord && !_lastConvertedWithBorder) return _sb.ToString();

        _sb.Clear();
        string[][] textStrings = text.Select(GetWithGraphics).ToArray();
        int lines = textStrings.Max(t => t.Length);

        for (int i = 0; i < lines; i++) {
            foreach (string[] textString in textStrings) {
                _sb.Append(i < textString.Length
                        ? textString[i]
                        : new string(' ', textString[0].Length)// Handle missing lines for alignment
                );
            }

            _sb.AppendLine();
        }

        _lastConvertedWord = text;
        _lastConvertedWithBorder = false;
        return _sb.ToString();
    }

    public string ConvertToAsciiStringWithBorder(string text) {
        if (_config.Border is null) return ConvertToAsciiString(text);
        if (text == _lastConvertedWord && _lastConvertedWithBorder) return _sb.ToString();
        
        ImmutableArray<string> cornerTopLeft = _config.Border.BorderGraphics[BorderAssignment.CornerTopLeft];
        ImmutableArray<string> cornerTopRight = _config.Border.BorderGraphics[BorderAssignment.CornerTopRight];
        ImmutableArray<string> cornerBottomLeft = _config.Border.BorderGraphics[BorderAssignment.CornerBottomLeft];
        ImmutableArray<string> cornerBottomRight = _config.Border.BorderGraphics[BorderAssignment.CornerBottomRight];
        ImmutableArray<string> straightTop = _config.Border.BorderGraphics[BorderAssignment.StraightTop];
        ImmutableArray<string> straightBottom = _config.Border.BorderGraphics[BorderAssignment.StraightBottom];
        ImmutableArray<string> straightLeft = _config.Border.BorderGraphics[BorderAssignment.StraightLeft];
        ImmutableArray<string> straightRight = _config.Border.BorderGraphics[BorderAssignment.StraightRight];
        
        int chunkSize = cornerTopLeft[0].Length; // In AsciiBorder we make sure everything is the same length

        ImmutableArray<string>[] colorLessTextArray = text.Select(GetWithoutGraphics).ToArray();

        // Calculate the necessary width of the text content
        int totalLineLength = colorLessTextArray.Sum(lines => lines[0].Length);

        // Adjust total line length to be a multiple of chunkSize
        int remainderWidth = totalLineLength % chunkSize;
        if (remainderWidth != 0) totalLineLength += chunkSize - remainderWidth;
        int straightSteps = totalLineLength / chunkSize;

        // Calculate text height
        string[][] textArray = text.Select(GetWithGraphics).ToArray();
        int textHeight = textArray[0].Length;
    
        // Adjust text height to be a multiple of chunkSize
        int remainderHeight = textHeight % chunkSize;
        if (remainderHeight != 0) textHeight += chunkSize - remainderHeight;

        _sb.Clear();
        
        // Construct the top border
        for (int i = 0; i < chunkSize; i++) {
            _sb.Append(cornerTopLeft[i]);
            for (int j = 0; j < straightSteps; j++) {
                _sb.Append(straightTop[i]);
            }
            _sb.AppendLine(cornerTopRight[i]);
        }
        
        
        // Construct the text with side borders
        for (int i = 0; i < textHeight; i++) {
            _sb.Append(straightLeft[i % chunkSize]);
            
            foreach (string[] textString in textArray) {
                _sb.Append(i < textString.Length
                        ? textString[i]
                        : new string(' ', textString[0].Length)// Handle missing lines for alignment
                );
            }

            _sb.AppendLine(straightRight[i % chunkSize]);
        }
        
        // Construct the bottom border
        for (int i = 0; i < chunkSize; i++) {
            _sb.Append(cornerBottomLeft[i]);
            for (int j = 0; j < straightSteps; j++) {
                _sb.Append(straightBottom[i]);
            }
            _sb.AppendLine(cornerBottomRight[i]);
        }
        
        _lastConvertedWord = text;
        _lastConvertedWithBorder = true;
        return _sb.ToString();
    }

}
