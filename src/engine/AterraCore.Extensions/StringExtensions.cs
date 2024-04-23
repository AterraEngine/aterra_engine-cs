// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringExtensions {
    public static bool IsNotNullOrEmpty(this string? str) {
        return !string.IsNullOrEmpty(str);
    }

    public static bool IsEmpty(this string[] arr) {
        return arr.Length == 0;
    }
    
    public static bool IsEmpty(this IEnumerable<string> arr) {
        return !arr.Any();
    }
}