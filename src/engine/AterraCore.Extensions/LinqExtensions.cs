// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinqExtensions {
    public static void ForEach<T>(this T[] array, Action<T> action) where T : notnull {
        foreach (T t in array) {
            action(t);
        }
    }
    
    public static void ForEach<T>(this LinkedList<T> linkedList, Action<T> action) where T : notnull {
        foreach (T t in linkedList) {
            action(t);
        }
    }
}