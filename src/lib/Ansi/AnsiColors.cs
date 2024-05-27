// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides a collection of ANSI colors represented as RGB byte values.
/// </summary>
public static class AnsiColors {
    /// <summary>
    ///     An instance of the <see cref="Dictionary{TKey,TValue}" /> class that maps strings to <see cref="ByteVector3" />
    ///     objects.
    /// </summary>
    private static readonly Dictionary<string, ByteVector3> Dictionary = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    /// The AnsiColors class provides a static dictionary of ANSI color names and their corresponding RGB byte values.
    /// /
    static AnsiColors() {
        Dictionary.Add("aliceblue", new ByteVector3(240, 248, 255));
        Dictionary.Add("antiquewhite", new ByteVector3(250, 235, 215));
        Dictionary.Add("aqua", new ByteVector3(0, 255, 255));
        Dictionary.Add("aquamarine", new ByteVector3(127, 255, 212));
        Dictionary.Add("azure", new ByteVector3(240, 255, 255));
        Dictionary.Add("beige", new ByteVector3(245, 245, 220));
        Dictionary.Add("bisque", new ByteVector3(255, 228, 196));
        Dictionary.Add("black", new ByteVector3(0, 0, 0));
        Dictionary.Add("blanchedalmond", new ByteVector3(255, 235, 205));
        Dictionary.Add("blue", new ByteVector3(0, 0, 255));
        Dictionary.Add("blueviolet", new ByteVector3(138, 43, 226));
        Dictionary.Add("brown", new ByteVector3(165, 42, 42));
        Dictionary.Add("burlywood", new ByteVector3(222, 184, 135));
        Dictionary.Add("cadetblue", new ByteVector3(95, 158, 160));
        Dictionary.Add("chartreuse", new ByteVector3(127, 255, 0));
        Dictionary.Add("chocolate", new ByteVector3(210, 105, 30));
        Dictionary.Add("coral", new ByteVector3(255, 127, 80));
        Dictionary.Add("cornflowerblue", new ByteVector3(100, 149, 237));
        Dictionary.Add("cornsilk", new ByteVector3(255, 248, 220));
        Dictionary.Add("crimson", new ByteVector3(220, 20, 60));
        Dictionary.Add("cyan", new ByteVector3(0, 255, 255));
        Dictionary.Add("darkblue", new ByteVector3(0, 0, 139));
        Dictionary.Add("darkcyan", new ByteVector3(0, 139, 139));
        Dictionary.Add("darkgoldenrod", new ByteVector3(184, 134, 11));
        Dictionary.Add("darkgray", new ByteVector3(169, 169, 169));
        Dictionary.Add("darkgreen", new ByteVector3(0, 100, 0));
        Dictionary.Add("darkkhaki", new ByteVector3(189, 183, 107));
        Dictionary.Add("darkmagenta", new ByteVector3(139, 0, 139));
        Dictionary.Add("darkolivegreen", new ByteVector3(85, 107, 47));
        Dictionary.Add("darkorange", new ByteVector3(255, 140, 0));
        Dictionary.Add("darkorchid", new ByteVector3(153, 50, 204));
        Dictionary.Add("darkred", new ByteVector3(139, 0, 0));
        Dictionary.Add("darksalmon", new ByteVector3(233, 150, 122));
        Dictionary.Add("darkseagreen", new ByteVector3(143, 188, 143));
        Dictionary.Add("darkslateblue", new ByteVector3(72, 61, 139));
        Dictionary.Add("darkslategray", new ByteVector3(47, 79, 79));
        Dictionary.Add("darkturquoise", new ByteVector3(0, 206, 209));
        Dictionary.Add("darkviolet", new ByteVector3(148, 0, 211));
        Dictionary.Add("deeppink", new ByteVector3(255, 20, 147));
        Dictionary.Add("deepskyblue", new ByteVector3(0, 191, 255));
        Dictionary.Add("dimgray", new ByteVector3(105, 105, 105));
        Dictionary.Add("dodgerblue", new ByteVector3(30, 144, 255));
        Dictionary.Add("firebrick", new ByteVector3(178, 34, 34));
        Dictionary.Add("floralwhite", new ByteVector3(255, 250, 240));
        Dictionary.Add("forestgreen", new ByteVector3(34, 139, 34));
        Dictionary.Add("gainsboro", new ByteVector3(220, 220, 220));
        Dictionary.Add("ghostwhite", new ByteVector3(248, 248, 255));
        Dictionary.Add("gold", new ByteVector3(255, 215, 0));
        Dictionary.Add("goldenrod", new ByteVector3(218, 165, 32));
        Dictionary.Add("gray", new ByteVector3(128, 128, 128));
        Dictionary.Add("green", new ByteVector3(0, 128, 0));
        Dictionary.Add("greenyellow", new ByteVector3(173, 255, 47));
        Dictionary.Add("honeydew", new ByteVector3(240, 255, 240));
        Dictionary.Add("hotpink", new ByteVector3(255, 105, 180));
        Dictionary.Add("indianred", new ByteVector3(205, 92, 92));
        Dictionary.Add("indigo", new ByteVector3(75, 0, 130));
        Dictionary.Add("ivory", new ByteVector3(255, 255, 240));
        Dictionary.Add("khaki", new ByteVector3(240, 230, 140));
        Dictionary.Add("lavender", new ByteVector3(230, 230, 250));
        Dictionary.Add("lavenderblush", new ByteVector3(255, 240, 245));
        Dictionary.Add("lawngreen", new ByteVector3(124, 252, 0));
        Dictionary.Add("lemonchiffon", new ByteVector3(255, 250, 205));
        Dictionary.Add("lightblue", new ByteVector3(173, 216, 230));
        Dictionary.Add("lightcoral", new ByteVector3(240, 128, 128));
        Dictionary.Add("lightcyan", new ByteVector3(224, 255, 255));
        Dictionary.Add("lightgoldenrodyellow", new ByteVector3(250, 250, 210));
        Dictionary.Add("lightgray", new ByteVector3(211, 211, 211));
        Dictionary.Add("lightgreen", new ByteVector3(144, 238, 144));
        Dictionary.Add("lightpink", new ByteVector3(255, 182, 193));
        Dictionary.Add("lightsalmon", new ByteVector3(255, 160, 122));
        Dictionary.Add("lightseagreen", new ByteVector3(32, 178, 170));
        Dictionary.Add("lightskyblue", new ByteVector3(135, 206, 250));
        Dictionary.Add("lightslategray", new ByteVector3(119, 136, 153));
        Dictionary.Add("lightsteelblue", new ByteVector3(176, 196, 222));
        Dictionary.Add("lightyellow", new ByteVector3(255, 255, 224));
        Dictionary.Add("lime", new ByteVector3(0, 255, 0));
        Dictionary.Add("limegreen", new ByteVector3(50, 205, 50));
        Dictionary.Add("linen", new ByteVector3(250, 240, 230));
        Dictionary.Add("magenta", new ByteVector3(255, 0, 255));
        Dictionary.Add("maroon", new ByteVector3(128, 0, 0));
        Dictionary.Add("mediumaquamarine", new ByteVector3(102, 205, 170));
        Dictionary.Add("mediumblue", new ByteVector3(0, 0, 205));
        Dictionary.Add("mediumorchid", new ByteVector3(186, 85, 211));
        Dictionary.Add("mediumpurple", new ByteVector3(147, 112, 219));
        Dictionary.Add("mediumseagreen", new ByteVector3(60, 179, 113));
        Dictionary.Add("mediumslateblue", new ByteVector3(123, 104, 238));
        Dictionary.Add("mediumspringgreen", new ByteVector3(0, 250, 154));
        Dictionary.Add("mediumturquoise", new ByteVector3(72, 209, 204));
        Dictionary.Add("mediumvioletred", new ByteVector3(199, 21, 133));
        Dictionary.Add("midnightblue", new ByteVector3(25, 25, 112));
        Dictionary.Add("mintcream", new ByteVector3(245, 255, 250));
        Dictionary.Add("mistyrose", new ByteVector3(255, 228, 225));
        Dictionary.Add("moccasin", new ByteVector3(255, 228, 181));
        Dictionary.Add("navajowhite", new ByteVector3(255, 222, 173));
        Dictionary.Add("navy", new ByteVector3(0, 0, 128));
        Dictionary.Add("oldlace", new ByteVector3(253, 245, 230));
        Dictionary.Add("olive", new ByteVector3(128, 128, 0));
        Dictionary.Add("olivedrab", new ByteVector3(107, 142, 35));
        Dictionary.Add("orange", new ByteVector3(255, 165, 0));
        Dictionary.Add("orangered", new ByteVector3(255, 69, 0));
        Dictionary.Add("orchid", new ByteVector3(218, 112, 214));
        Dictionary.Add("palegoldenrod", new ByteVector3(238, 232, 170));
        Dictionary.Add("palegreen", new ByteVector3(152, 251, 152));
        Dictionary.Add("paleturquoise", new ByteVector3(175, 238, 238));
        Dictionary.Add("palevioletred", new ByteVector3(219, 112, 147));
        Dictionary.Add("papayawhip", new ByteVector3(255, 239, 213));
        Dictionary.Add("peachpuff", new ByteVector3(255, 218, 185));
        Dictionary.Add("peru", new ByteVector3(205, 133, 63));
        Dictionary.Add("pink", new ByteVector3(255, 192, 203));
        Dictionary.Add("plum", new ByteVector3(221, 160, 221));
        Dictionary.Add("powderblue", new ByteVector3(176, 224, 230));
        Dictionary.Add("purple", new ByteVector3(128, 0, 128));
        Dictionary.Add("red", new ByteVector3(255, 0, 0));
        Dictionary.Add("rose", new ByteVector3(255, 97, 136));
        Dictionary.Add("rosybrown", new ByteVector3(188, 143, 143));
        Dictionary.Add("royalblue", new ByteVector3(65, 105, 225));
        Dictionary.Add("saddlebrown", new ByteVector3(139, 69, 19));
        Dictionary.Add("salmon", new ByteVector3(250, 128, 114));
        Dictionary.Add("sandybrown", new ByteVector3(244, 164, 96));
        Dictionary.Add("seagreen", new ByteVector3(46, 139, 87));
        Dictionary.Add("seashell", new ByteVector3(255, 245, 238));
        Dictionary.Add("sienna", new ByteVector3(160, 82, 45));
        Dictionary.Add("silver", new ByteVector3(192, 192, 192));
        Dictionary.Add("skyblue", new ByteVector3(135, 206, 235));
        Dictionary.Add("slateblue", new ByteVector3(106, 90, 205));
        Dictionary.Add("slategray", new ByteVector3(112, 128, 144));
        Dictionary.Add("snow", new ByteVector3(255, 250, 250));
        Dictionary.Add("springgreen", new ByteVector3(0, 255, 127));
        Dictionary.Add("steelblue", new ByteVector3(70, 130, 180));
        Dictionary.Add("tan", new ByteVector3(210, 180, 140));
        Dictionary.Add("teal", new ByteVector3(0, 128, 128));
        Dictionary.Add("thistle", new ByteVector3(216, 191, 216));
        Dictionary.Add("tomato", new ByteVector3(255, 99, 71));
        Dictionary.Add("turquoise", new ByteVector3(64, 224, 208));
        Dictionary.Add("violet", new ByteVector3(238, 130, 238));
        Dictionary.Add("wheat", new ByteVector3(245, 222, 179));
        Dictionary.Add("white", new ByteVector3(255, 255, 255));
        Dictionary.Add("whitesmoke", new ByteVector3(245, 245, 245));
        Dictionary.Add("yellow", new ByteVector3(255, 255, 0));
        Dictionary.Add("yellowgreen", new ByteVector3(154, 205, 50));
    }

    /// <summary>
    ///     Gets a read-only dictionary containing known colors with their corresponding RGB values.
    /// </summary>
    /// <value>
    ///     The known colors dictionary.
    /// </value>
    public static ReadOnlyDictionary<string, ByteVector3> KnownColorsDictionary => Dictionary.AsReadOnly();

    /// <summary>
    ///     Retrieves the color value associated with the given color name from the dictionary.
    /// </summary>
    /// <param name="colorName">The name of the color to retrieve.</param>
    /// <returns>
    ///     The color value associated with the specified color name, or ByteVector3.Zero if the color name is not found
    ///     in the dictionary.
    /// </returns>
    public static ByteVector3 GetColor(string colorName) =>
        Dictionary.TryGetValue(colorName, out ByteVector3 color)
            ? color
            : ByteVector3.Zero; // Handle the case where the color name is not found (e.g., return a default color)

    /// <summary>
    ///     Tries to retrieve a color from the dictionary based on the specified color name.
    /// </summary>
    /// <param name="colorName">The name of the color to retrieve.</param>
    /// <param name="color">
    ///     When this method returns, contains the color associated with the specified color name if it is
    ///     found; otherwise, contains a default color.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the color with the specified color name is found in the dictionary; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryGetColor(string colorName, out ByteVector3 color) {
        color = ByteVector3.Zero;
        return Dictionary.TryGetValue(colorName, out color);
    }

    /// <summary>
    ///     Adds a color to the dictionary.
    /// </summary>
    /// <param name="colorName">The name of the color to add.</param>
    /// <param name="value">The RGB value of the color to add.</param>
    /// <returns>Returns true if the color was added successfully, false otherwise.</returns>
    public static bool AddColor(string colorName, ByteVector3 value) => Dictionary.TryAdd(colorName, value);
}