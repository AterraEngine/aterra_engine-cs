// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;

namespace Ansi;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AnsiColors { 
    private static readonly Dictionary<string, ByteVector3> _dictionary = new();
    public static ReadOnlyDictionary<string, ByteVector3> KnownColorsDictionary => _dictionary.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    static AnsiColors() {
        _dictionary.Add("maroon",                   new ByteVector3(128,0,0));
        _dictionary.Add("darkred",                  new ByteVector3(139,0,0));
        _dictionary.Add("brown",                    new ByteVector3(165,42,42));
        _dictionary.Add("firebrick",                new ByteVector3(178,34,34));
        _dictionary.Add("crimson",                  new ByteVector3(220,20,60));
        _dictionary.Add("red",                      new ByteVector3(255,0,0));
        _dictionary.Add("tomato",                   new ByteVector3(255,99,71));
        _dictionary.Add("coral",                    new ByteVector3(255,127,80));
        _dictionary.Add("indianred",                new ByteVector3(205,92,92));
        _dictionary.Add("lightcoral",               new ByteVector3(240,128,128));
        _dictionary.Add("darksalmon",               new ByteVector3(233,150,122));
        _dictionary.Add("salmon",                   new ByteVector3(250,128,114));
        _dictionary.Add("lightsalmon",              new ByteVector3(255,160,122));
        _dictionary.Add("orangered",                new ByteVector3(255,69,0));
        _dictionary.Add("darkorange",               new ByteVector3(255,140,0));
        _dictionary.Add("orange",                   new ByteVector3(255,165,0));
        _dictionary.Add("gold",                     new ByteVector3(255,215,0));
        _dictionary.Add("darkgoldenrod",            new ByteVector3(184,134,11));
        _dictionary.Add("goldenrod",                new ByteVector3(218,165,32));
        _dictionary.Add("palegoldenrod",            new ByteVector3(238,232,170));
        _dictionary.Add("darkkhaki",                new ByteVector3(189,183,107));
        _dictionary.Add("khaki",                    new ByteVector3(240,230,140));
        _dictionary.Add("olive",                    new ByteVector3(128,128,0));
        _dictionary.Add("yellow",                   new ByteVector3(255,255,0));
        _dictionary.Add("yellowgreen",              new ByteVector3(154,205,50));
        _dictionary.Add("darkolivegreen",           new ByteVector3(85,107,47));
        _dictionary.Add("olivedrab",                new ByteVector3(107,142,35));
        _dictionary.Add("lawngreen",                new ByteVector3(124,252,0));
        _dictionary.Add("chartreuse",               new ByteVector3(127,255,0));
        _dictionary.Add("greenyellow",              new ByteVector3(173,255,47));
        _dictionary.Add("darkgreen",                new ByteVector3(0,100,0));
        _dictionary.Add("green",                    new ByteVector3(0,128,0));
        _dictionary.Add("forestgreen",              new ByteVector3(34,139,34));
        _dictionary.Add("lime",                     new ByteVector3(0,255,0));
        _dictionary.Add("limegreen",                new ByteVector3(50,205,50));
        _dictionary.Add("lightgreen",               new ByteVector3(144,238,144));
        _dictionary.Add("palegreen",                new ByteVector3(152,251,152));
        _dictionary.Add("darkseagreen",             new ByteVector3(143,188,143));
        _dictionary.Add("mediumspringgreen",        new ByteVector3(0,250,154));
        _dictionary.Add("springgreen",              new ByteVector3(0,255,127));
        _dictionary.Add("seagreen",                 new ByteVector3(46,139,87));
        _dictionary.Add("mediumaquamarine",         new ByteVector3(102,205,170));
        _dictionary.Add("mediumseagreen",           new ByteVector3(60,179,113));
        _dictionary.Add("lightseagreen",            new ByteVector3(32,178,170));
        _dictionary.Add("darkslategray",            new ByteVector3(47,79,79));
        _dictionary.Add("teal",                     new ByteVector3(0,128,128));
        _dictionary.Add("darkcyan",                 new ByteVector3(0,139,139));
        _dictionary.Add("aqua",                     new ByteVector3(0,255,255));
        _dictionary.Add("cyan",                     new ByteVector3(0,255,255));
        _dictionary.Add("lightcyan",                new ByteVector3(224,255,255));
        _dictionary.Add("darkturquoise",            new ByteVector3(0,206,209));
        _dictionary.Add("turquoise",                new ByteVector3(64,224,208));
        _dictionary.Add("mediumturquoise",          new ByteVector3(72,209,204));
        _dictionary.Add("paleturquoise",            new ByteVector3(175,238,238));
        _dictionary.Add("aquamarine",               new ByteVector3(127,255,212));
        _dictionary.Add("powderblue",               new ByteVector3(176,224,230));
        _dictionary.Add("cadetblue",                new ByteVector3(95,158,160));
        _dictionary.Add("steelblue",                new ByteVector3(70,130,180));
        _dictionary.Add("cornflowerblue",           new ByteVector3(100,149,237));
        _dictionary.Add("deepskyblue",              new ByteVector3(0,191,255));
        _dictionary.Add("dodgerblue",               new ByteVector3(30,144,255));
        _dictionary.Add("lightblue",                new ByteVector3(173,216,230));
        _dictionary.Add("skyblue",                  new ByteVector3(135,206,235));
        _dictionary.Add("lightskyblue",             new ByteVector3(135,206,250));
        _dictionary.Add("midnightblue",             new ByteVector3(25,25,112));
        _dictionary.Add("navy",                     new ByteVector3(0,0,128));
        _dictionary.Add("darkblue",                 new ByteVector3(0,0,139));
        _dictionary.Add("mediumblue",               new ByteVector3(0,0,205));
        _dictionary.Add("blue",                     new ByteVector3(0,0,255));
        _dictionary.Add("royalblue",                new ByteVector3(65,105,225));
        _dictionary.Add("blueviolet",               new ByteVector3(138,43,226));
        _dictionary.Add("indigo",                   new ByteVector3(75,0,130));
        _dictionary.Add("darkslateblue",            new ByteVector3(72,61,139));
        _dictionary.Add("slateblue",                new ByteVector3(106,90,205));
        _dictionary.Add("mediumslateblue",          new ByteVector3(123,104,238));
        _dictionary.Add("mediumpurple",             new ByteVector3(147,112,219));
        _dictionary.Add("darkmagenta",              new ByteVector3(139,0,139));
        _dictionary.Add("darkviolet",               new ByteVector3(148,0,211));
        _dictionary.Add("darkorchid",               new ByteVector3(153,50,204));
        _dictionary.Add("mediumorchid",             new ByteVector3(186,85,211));
        _dictionary.Add("purple",                   new ByteVector3(128,0,128));
        _dictionary.Add("thistle",                  new ByteVector3(216,191,216));
        _dictionary.Add("plum",                     new ByteVector3(221,160,221));
        _dictionary.Add("violet",                   new ByteVector3(238,130,238));
        _dictionary.Add("magenta",                  new ByteVector3(255,0,255));
        _dictionary.Add("orchid",                   new ByteVector3(218,112,214));
        _dictionary.Add("mediumvioletred",          new ByteVector3(199,21,133));
        _dictionary.Add("palevioletred",            new ByteVector3(219,112,147));
        _dictionary.Add("deeppink",                 new ByteVector3(255,20,147));
        _dictionary.Add("hotpink",                  new ByteVector3(255,105,180));
        _dictionary.Add("lightpink",                new ByteVector3(255,182,193));
        _dictionary.Add("pink",                     new ByteVector3(255,192,203));
        _dictionary.Add("antiquewhite",             new ByteVector3(250,235,215));
        _dictionary.Add("beige",                    new ByteVector3(245,245,220));
        _dictionary.Add("bisque",                   new ByteVector3(255,228,196));
        _dictionary.Add("blanchedalmond",	        new ByteVector3(255,235,205));
        _dictionary.Add("wheat",                    new ByteVector3(245,222,179));
        _dictionary.Add("cornsilk",                 new ByteVector3(255,248,220));
        _dictionary.Add("lemonchiffon",             new ByteVector3(255,250,205));
        _dictionary.Add("lightgoldenrodyellow",     new ByteVector3(250,250,210));
        _dictionary.Add("lightyellow",              new ByteVector3(255,255,224));
        _dictionary.Add("saddlebrown",              new ByteVector3(139,69,19));
        _dictionary.Add("sienna",                   new ByteVector3(160,82,45));
        _dictionary.Add("chocolate",                new ByteVector3(210,105,30));
        _dictionary.Add("peru",                     new ByteVector3(205,133,63));
        _dictionary.Add("sandybrown",               new ByteVector3(244,164,96));
        _dictionary.Add("burlywood",                new ByteVector3(222,184,135));
        _dictionary.Add("tan",                      new ByteVector3(210,180,140));
        _dictionary.Add("rosybrown",                new ByteVector3(188,143,143));
        _dictionary.Add("moccasin",                 new ByteVector3(255,228,181));
        _dictionary.Add("navajowhite",              new ByteVector3(255,222,173));
        _dictionary.Add("peachpuff",                new ByteVector3(255,218,185));
        _dictionary.Add("mistyrose",                new ByteVector3(255,228,225));
        _dictionary.Add("lavenderblush",            new ByteVector3(255,240,245));
        _dictionary.Add("linen",                    new ByteVector3(250,240,230));
        _dictionary.Add("oldlace",                  new ByteVector3(253,245,230));
        _dictionary.Add("papayawhip",               new ByteVector3(255,239,213));
        _dictionary.Add("seashell",                 new ByteVector3(255,245,238));
        _dictionary.Add("mintcream",                new ByteVector3(245,255,250));
        _dictionary.Add("slategray",                new ByteVector3(112,128,144));
        _dictionary.Add("lightslategray",           new ByteVector3(119,136,153));
        _dictionary.Add("lightsteelblue",           new ByteVector3(176,196,222));
        _dictionary.Add("lavender",                 new ByteVector3(230,230,250));
        _dictionary.Add("floralwhite",              new ByteVector3(255,250,240));
        _dictionary.Add("aliceblue",                new ByteVector3(240,248,255));
        _dictionary.Add("ghostwhite",               new ByteVector3(248,248,255));
        _dictionary.Add("honeydew",                 new ByteVector3(240,255,240));
        _dictionary.Add("ivory",                    new ByteVector3(255,255,240));
        _dictionary.Add("azure",                    new ByteVector3(240,255,255));
        _dictionary.Add("snow",                     new ByteVector3(255,250,250));
        _dictionary.Add("black",                    new ByteVector3(0,0,0));
        _dictionary.Add("dimgray",                  new ByteVector3(105,105,105));
        _dictionary.Add("gray",	                    new ByteVector3(128,128,128));
        _dictionary.Add("darkgray",	                new ByteVector3(169,169,169));
        _dictionary.Add("silver",	                new ByteVector3(192,192,192));
        _dictionary.Add("lightgray",	            new ByteVector3(211,211,211));
        _dictionary.Add("gainsboro",                new ByteVector3(220,220,220));
        _dictionary.Add("whitesmoke",               new ByteVector3(245,245,245));
        _dictionary.Add("white",                    new ByteVector3(255,255,255));
    }
    
    public static ByteVector3 GetColor(string colorName) {
        return _dictionary.TryGetValue(colorName, out var color) 
            ? color 
            : ByteVector3.Zero; // Handle the case where the color name is not found (e.g., return a default color)
    }

    public static bool TryGetColor(string colorName, out ByteVector3 color) {
        color = ByteVector3.Zero;
        return _dictionary.TryGetValue(colorName, out color);
    }
    
    public static bool AddColor(string colorName, ByteVector3 value) {
        return _dictionary.TryAdd(colorName, value);
    }
}