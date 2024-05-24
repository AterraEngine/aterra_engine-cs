// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;
using System.Xml.Serialization;
namespace AterraCore.Common;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISemanticVersion {
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
}

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
public partial struct SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>, ISemanticVersion {
    [XmlIgnore] public int Major { get; set; }
    [XmlIgnore] public int Minor { get; set; }
    [XmlIgnore] public int Patch { get; set; }
    [XmlIgnore] public string? Addendum { get; set; }

    [XmlAttribute("value")]
    public string Value {
        get => ToString();
        set => ParseFromString(value);
    }

    private static readonly Regex Regex = MyRegex();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Creates a new instance of the SemanticVersion class with the specified version numbers.
    /// </summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="patch">The patch version number.</param>
    public SemanticVersion(int major, int minor, int patch) {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    /// <summary>
    ///     Initializes a new instance of the SemanticVersion class using the specified version string.
    /// </summary>
    /// <param name="version">A string representing the version number.</param>
    public SemanticVersion(string version) {
        ParseFromString(version);
    }

    public static SemanticVersion Zero => new(0, 0, 0);
    public static SemanticVersion Max => new(int.MaxValue, int.MaxValue, int.MaxValue);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Parses the version string and sets the major, minor, and patch numbers.
    /// </summary>
    /// <param name="version">The version string in the format 'Major.Minor.Patch'.</param>
    /// <exception cref="ArgumentException">Thrown when the version string is in an invalid format.</exception>
    private void ParseFromString(string version) {
        Match match = Regex.Match(version);
        if (!match.Success) {
            throw new ArgumentException("Invalid version format. Expected format: 'Major.Minor.Patch'");
        }

        Major = int.Parse(match.Groups[1].Value);
        Minor = int.Parse(match.Groups[2].Value);
        Patch = int.Parse(match.Groups[3].Value);
        Addendum = match.Groups[4].Success ? match.Groups[4].Value : null;
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    ///     A string that represents the current object. The string format is "{Major}.{Minor}.{Patch}".
    /// </returns>
    public override string ToString() =>
        Addendum != null
            ? $"{Major}.{Minor}.{Patch}-{Addendum}"
            : $"{Major}.{Minor}.{Patch}";

    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public bool Equals(SemanticVersion other) => Major == other.Major && Minor == other.Minor && Patch == other.Patch;
    public override bool Equals(object? obj) => obj is SemanticVersion version && Equals(version);

    public int CompareTo(SemanticVersion other) {
        if (Major != other.Major) {
            return Major.CompareTo(other.Major);
        }

        if (Minor != other.Minor) {
            return Minor.CompareTo(other.Minor);
        }

        if (Patch != other.Patch) {
            return Patch.CompareTo(other.Patch);
        }

        if (Addendum == null && other.Addendum != null) {
            return -1;
        }

        if (Addendum != null && other.Addendum == null) {
            return 1;
        }

        return 0;
    }
    public override int GetHashCode() => (Major, Minor, Patch, Addendum).GetHashCode();

    // Overriding operators
    public static bool operator >(SemanticVersion left, SemanticVersion right) => left.CompareTo(right) > 0;
    public static bool operator <(SemanticVersion left, SemanticVersion right) => left.CompareTo(right) < 0;
    public static bool operator ==(SemanticVersion left, SemanticVersion right) => left.Equals(right);
    public static bool operator !=(SemanticVersion left, SemanticVersion right) => !left.Equals(right);

    [GeneratedRegex(@"^(\d+)\.(\d+)\.(\d+)(?:\-(\w*))?$")]
    private static partial Regex MyRegex();
}