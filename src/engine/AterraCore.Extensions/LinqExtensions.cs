// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinqExtensions {
    public static void IterateOver<T>(this IEnumerable<T> array, Action<T> action) where T : notnull {
        foreach (T t in array) {
            action(t);
        }
    }
    
    public static void IterateOver<T>(this IEnumerable<T> array, Func<T,T> action) where T : notnull {
        foreach (T t in array) {
            action(t);
        }
    }
    
    public static void IterateOver<T>(this LinkedList<T> linkedList, Action<T> action) where T : notnull {
        foreach (T t in linkedList) {
            action(t);
        }
    }
}