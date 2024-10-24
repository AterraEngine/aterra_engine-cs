// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;

namespace AterraEngine.Generators.Helpers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ListPooledObjectPolicy<T> : PooledObjectPolicy<List<T>> {
    public override List<T> Create() => [];
    public override bool Return(List<T> obj) {
        obj.Clear();
        return true;
    }

    public static DefaultObjectPool<List<T>> CreatePool() => new(new ListPooledObjectPolicy<T>());
}
