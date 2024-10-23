// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;

namespace Ascii;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AsciiBorder {
    // ReSharper disable once InconsistentNaming
    protected virtual Dictionary<BorderAssignment, ImmutableArray<string>> _borderGraphics { get; init; } = new();

    public IReadOnlyDictionary<BorderAssignment, ImmutableArray<string>> BorderGraphics => _borderGraphics.AsReadOnly();

    public AsciiBorder() {
        EnsureAllBordersDefined();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddBorderGraphics(BorderAssignment assignment, string[] asciiArt) {
        switch (assignment) {
            case BorderAssignment.StraightHorizontal:
                _borderGraphics.Add(BorderAssignment.StraightBottom, [..asciiArt]);
                _borderGraphics.Add(BorderAssignment.StraightTop, [..asciiArt]);
                break;
            case BorderAssignment.StraightVertical:
                _borderGraphics.Add(BorderAssignment.StraightLeft, [..asciiArt]);
                _borderGraphics.Add(BorderAssignment.StraightRight, [..asciiArt]);
                break;

            case BorderAssignment.CornerTopLeft:
            case BorderAssignment.CornerTopRight:
            case BorderAssignment.CornerBottomLeft:
            case BorderAssignment.CornerBottomRight:
            case BorderAssignment.StraightTop:
            case BorderAssignment.StraightBottom:
            case BorderAssignment.StraightLeft:
            case BorderAssignment.StraightRight:
            default:
                _borderGraphics.Add(assignment, [..asciiArt]);
                EnsureAllBordersDefined();
                break;
        }
    }

    private void EnsureAllBordersDefined() {
        if (_borderGraphics.Count == 0) return;

        int maxWidth = _borderGraphics.Values.Max(border => border.Max(line => line.Length));
        int maxHeight = _borderGraphics.Values.Max(border => border.Length);

        foreach (BorderAssignment assignment in Enum.GetValues(typeof(BorderAssignment))) {
            if (!_borderGraphics.TryGetValue(assignment, out ImmutableArray<string> value)) {
                _borderGraphics[assignment] = GenerateEmptyBorder(maxWidth, maxHeight);
                continue;
            }

            _borderGraphics[assignment] = PadBorder(value, maxWidth, maxHeight);
        }
    }

    private static ImmutableArray<string> PadBorder(ImmutableArray<string> border, int width, int height) {
        List<string> paddedBorder = border.Select(t => t.PadRight(width)).ToList();

        for (int i = border.Length; i < height; i++) {
            paddedBorder.Add(new string(' ', width));
        }

        return [..paddedBorder];
    }

    private static ImmutableArray<string> GenerateEmptyBorder(int width, int height) {
        string[] emptyBorder = new string[height];

        for (int i = 0; i < height; i++) {
            emptyBorder[i] = new string(' ', width);
        }

        return [..emptyBorder];
    }
}
