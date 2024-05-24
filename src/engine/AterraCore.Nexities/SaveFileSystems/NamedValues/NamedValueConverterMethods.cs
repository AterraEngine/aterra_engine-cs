// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.SaveFileSystems.NamedValues;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class NamedValueConverterMethods {
    public static bool ConvertToVector2(string input, out Vector2 output) {
        output = default;
        int[] data = input.Split(';').Select(int.Parse).ToArray();
        if (data.Length != 2) return false;
        output = new Vector2(data[0], data[1]);
        return true;
    }
    
    public static bool ConvertToVector3(string input, out Vector3 output) {
        output = default;
        int[] data = input.Split(';').Select(int.Parse).ToArray();
        if (data.Length != 3) return false;
        output = new Vector3(data[0], data[1], data[2]);
        return true;
    }
    
    public static bool ConvertToGuid(string input,out Guid output) {
        output = default;
        if (!Guid.TryParse(input, out Guid result)) {
            return false;
        }
        output = result;
        return true;
    }
    
    public static bool ConvertToString(string input, out string output) {
        output = input;
        return true;
    }
}
