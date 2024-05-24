// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringExtensions {
    public static bool IsNotNullOrEmpty(this string? str) => !string.IsNullOrEmpty(str);

    public static bool IsEmpty(this string[] arr) => arr.Length == 0;

    public static bool IsEmpty(this IEnumerable<string> arr) => !arr.Any();
}