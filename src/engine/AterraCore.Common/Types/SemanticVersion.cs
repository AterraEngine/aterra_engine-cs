// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using CodeOfChaos.Extensions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AterraCore.Common.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a semantic version number.
/// </summary>
/// <remarks>
///     A semantic version number consists of three components: Major, Minor, and Patch.
///     The Major version number is incremented when incompatible changes are made.
///     The Minor version number is incremented when new, backwards-compatible features are added.
///     The Patch version number is incremented when backwards-compatible fixes are made.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct SemanticVersion(int major, int minor, int patch, string? addendum = null) : IComparable<SemanticVersion>, IEquatable<SemanticVersion> {
    [XmlIgnore] public int Major { get; } = major;
    [XmlIgnore] public int Minor { get; } = minor;
    [XmlIgnore] public int Patch { get; } = patch;
    [XmlIgnore] public string Addendum { get; } = addendum ?? string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the SemanticVersion class using the specified version string.
    /// </summary>
    /// <param name="version">A string representing the version number.</param>
    public SemanticVersion(string version) : this(0, 0, 0) {
        Match match = RegexLib.SemanticVersion.Match(version);
        if (!match.Success) throw new ArgumentException("Invalid version format. Expected format: 'Major.Minor.Patch'");

        Major = int.Parse(match.Groups[1].Value);
        Minor = int.Parse(match.Groups[2].Value);
        Patch = int.Parse(match.Groups[3].Value);
        Addendum = match.Groups[4].Success && match.Groups[4].Value.IsNotNullOrEmpty() ? match.Groups[4].Value : string.Empty;
    }

    public static readonly SemanticVersion Zero = new(0, 0, 0, string.Empty);
    public static readonly SemanticVersion Max = new(int.MaxValue, int.MaxValue, int.MaxValue, string.Empty);

    // -----------------------------------------------------------------------------------------------------------------
    // Implicit Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static implicit operator SemanticVersion(string s) => new(s);
    public static implicit operator string(SemanticVersion assetId) => assetId.ToString();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to parse a string representation of a semantic version and returns a value indicating whether the parsing was successful.
    /// </summary>
    /// <param name="input">The string representation of the semantic version.</param>
    /// <param name="version">When this method returns, contains the parsed SemanticVersion if parsing was successful, or SemanticVersion.Zero if parsing failed.</param>
    /// <returns><c>true</c> if parsing was successful; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string input, out SemanticVersion version) {
        version = Zero;
        Match match = RegexLib.SemanticVersion.Match(input);
        if (!match.Success) return false;

        version = new SemanticVersion(
            int.Parse(match.Groups[1].Value),
            int.Parse(match.Groups[2].Value),
            int.Parse(match.Groups[3].Value),
            match.Groups[4].Success && match.Groups[4].Value.IsNotNullOrEmpty() ? match.Groups[4].Value : string.Empty
        );
        return true;
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object. The string format is "{Major}.{Minor}.{Patch}".
    /// </returns>
    public override string ToString() =>
        Addendum.IsNotNullOrEmpty()
            ? $"{Major}.{Minor}.{Patch}-{Addendum}"
            : $"{Major}.{Minor}.{Patch}";

    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public bool Equals(SemanticVersion other) => Major == other.Major && Minor == other.Minor && Patch == other.Patch;
    public override bool Equals(object? obj) => obj is SemanticVersion version && Equals(version);

    public int CompareTo(SemanticVersion other) {
        if (Major != other.Major) return Major.CompareTo(other.Major);
        if (Minor != other.Minor) return Minor.CompareTo(other.Minor);
        if (Patch != other.Patch) return Patch.CompareTo(other.Patch);
        if (Addendum.IsNullOrEmpty() && other.Addendum.IsNotNullOrEmpty()) return -1;
        if (Addendum.IsNotNullOrEmpty() && other.Addendum.IsNullOrEmpty()) return 1;
        return 0;
    }
    public override int GetHashCode() => (Major, Minor, Patch, Addendum).GetHashCode();

    // Overriding operators
    public static bool operator >(SemanticVersion left, SemanticVersion right) => left.CompareTo(right) > 0;
    public static bool operator <(SemanticVersion left, SemanticVersion right) => left.CompareTo(right) < 0;
    public static bool operator ==(SemanticVersion left, SemanticVersion right) => left.Equals(right);
    public static bool operator !=(SemanticVersion left, SemanticVersion right) => !left.Equals(right);
    public static bool operator <=(SemanticVersion left, SemanticVersion right) => left.Equals(right) || left.CompareTo(right) < 0;
    public static bool operator >=(SemanticVersion left, SemanticVersion right) => left.Equals(right) || left.CompareTo(right) > 0;
}
