// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;

namespace Ascii.BorderLibrary;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ThinBorder : AsciiBorder {
    protected override Dictionary<BorderAssignment, ImmutableArray<string>> _borderGraphics { get; init; } = new() {
        [BorderAssignment.CornerTopLeft] = [
            "╔"
        ],
        [BorderAssignment.CornerTopRight] = [
            "╗"
        ],
        [BorderAssignment.CornerBottomRight] = [
            "╝"
        ],
        [BorderAssignment.CornerBottomLeft] = [
            "╚"
        ],
        [BorderAssignment.StraightLeft] = [
            "║"
        ],
        [BorderAssignment.StraightRight] = [
            "║"
        ],
        [BorderAssignment.StraightTop] = [
            "═"
        ],
        [BorderAssignment.StraightBottom] = [
            "═"
        ]
    };

    public static ThinBorder GeneratePaddingBorder(int padding) {
        // Check for valid padding
        if (padding < 1)
            throw new ArgumentException("Padding must be 1 or greater.", nameof(padding));

        string topBottomPadding = new('═', padding);// Horizontal line for top/bottom borders
        string spacePadding = new(' ', padding);// Generate the space padding

        return new ThinBorder {
            _borderGraphics = new Dictionary<BorderAssignment, ImmutableArray<string>> {
                [BorderAssignment.CornerTopLeft] = [
                    "╔" + topBottomPadding,
                    "║" + spacePadding
                ],
                [BorderAssignment.CornerTopRight] = [
                    topBottomPadding + "╗",
                    spacePadding + "║"
                ],
                [BorderAssignment.CornerBottomRight] = [
                    spacePadding + "║",
                    topBottomPadding + "╝"
                ],
                [BorderAssignment.CornerBottomLeft] = [
                    "║" + spacePadding,
                    "╚" + topBottomPadding
                ],
                [BorderAssignment.StraightLeft] = [..Enumerable.Repeat("║" + spacePadding, padding + 1)],
                [BorderAssignment.StraightRight] = [..Enumerable.Repeat(spacePadding + "║", padding + 1)],
                [BorderAssignment.StraightTop] = [
                    "═" + topBottomPadding,
                    " " + spacePadding
                ],
                [BorderAssignment.StraightBottom] = [
                    " " + spacePadding,
                    "═" + topBottomPadding
                ]
            }
        };
    }
}
