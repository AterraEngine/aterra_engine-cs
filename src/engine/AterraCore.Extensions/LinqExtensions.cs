// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinqExtensions {
    public static void IterateOver<T>(this IEnumerable<T> array, Action<T> action) where T : notnull {
        array.ToList().ForEach(action);
    }
    
    public static void IterateOver<T>(this IEnumerable<T> array, Func<T,T> action) where T : notnull {
        array.ToList().ForEach(a => action(a));
    }
    
    public static void IterateOver<T>(this LinkedList<T> linkedList, Action<T> action) where T : notnull {
        linkedList.ToList().ForEach(action);
    }
    public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> array, Func<T,bool> action) where T : notnull {
        return array.Where(a => !action(a));
    }
}