// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a three-dimensional vector with byte components.
/// </summary>
public struct ByteVector3(int x, int y, int z) {
    public byte X { get; } = (byte)x;
    public byte Y { get; } = (byte)y;
    public byte Z { get; } = (byte)z;
    // -----------------------------------------------------------------------------------------------------------------
    // Instance creators
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Represents a zero vector in 3D space.
    /// </summary>
    /// <value>
    ///     A <see cref="ByteVector3" /> object with all components set to zero.
    /// </value>
    public static ByteVector3 Zero => new(0, 0, 0);

    /// <summary>
    ///     Represents the maximum value for the ByteVector3 class.
    /// </summary>
    /// <remarks>
    ///     This property returns a new instance of the ByteVector3 class with each component set to its maximum value of 255.
    /// </remarks>
    /// <returns>A new instance of the ByteVector3 class with each component set to 255.</returns>
    public static ByteVector3 Max => new(255, 255, 255);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Converts the current object to an array of bytes.
    /// </summary>
    /// <returns>An array of bytes representing the object.</returns>
    public byte[] ToArray() => [X, Y, Z];

    /// <summary>
    ///     Converts the current instance to a string representation using ANSI format.
    /// </summary>
    /// <returns>A string that represents the current instance with ANSI format.</returns>
    public string ToAnsiString() => $"{X};{Y};{Z}";

    /// <summary>
    ///     Converts the X, Y, and Z values of the object to an RGB string representation.
    /// </summary>
    /// <returns>
    ///     A string representation of the RGB values in the format "rgb(X,Y,Z)".
    /// </returns>
    public string ToRgbString() => $"rgb({X},{Y},{Z})";
}