// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Ascii;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum BorderAssignment {
    CornerTopLeft,
    CornerTopRight,
    CornerBottomLeft,
    CornerBottomRight,

    StraightTop,
    StraightBottom,
    StraightLeft,
    StraightRight,

    StraightHorizontal,// helper -> StraightLeft, StraightRight
    StraightVertical// helper -> StraightTop, StraightBottom
}
