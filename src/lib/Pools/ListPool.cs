// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ListPool<T> {
    private static readonly ObjectPool<List<T>> Pool =
        new DefaultObjectPool<List<T>>(new DefaultPooledObjectPolicy<List<T>>());

    public static List<T> Get() => Pool.Get();

    public static void Return(List<T> list) {
        list.Clear();
        Pool.Return(list);
    }
}
