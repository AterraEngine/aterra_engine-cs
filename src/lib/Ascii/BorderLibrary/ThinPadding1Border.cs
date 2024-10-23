// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;

namespace Ascii.BorderLibrary;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ThinPadding1Border :AsciiBorder {
    protected override Dictionary<BorderAssignment, ImmutableArray<string>> _borderGraphics { get; init; } = new() {
        [BorderAssignment.CornerTopLeft] = [
            "╔═",
            "║ "
        ],
        [BorderAssignment.CornerTopRight] = [
            "═╗",
            " ║"
        ],
        [BorderAssignment.CornerBottomRight] = [
            " ║",
            "═╝"
        ],
        [BorderAssignment.CornerBottomLeft] = [
            "║ ",
            "╚═"
            
        ],
        [BorderAssignment.StraightLeft] = [
            "║ ",
            "║ "
        ],
        [BorderAssignment.StraightRight] = [
            " ║",
            " ║"
        ],
        [BorderAssignment.StraightTop] = [
            "══",
            "  "
        ],
        [BorderAssignment.StraightBottom] = [
            "  ",
            "══"
        ],
    };
}
