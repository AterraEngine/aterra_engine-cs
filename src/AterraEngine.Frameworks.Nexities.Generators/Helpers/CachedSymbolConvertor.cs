// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AterraEngine.Frameworks.Nexities.Generators.Helpers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CachedSymbolConvertor : ICollection {
    private readonly List<string> _keys = [];
    private Dictionary<string, INamedTypeSymbol?> _cache = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Add(string key) => _keys.Add(key);
    public INamedTypeSymbol GetSymbol(string name) => _cache[name]!;
    public bool EqualsSymbol(INamedTypeSymbol left, string right) => SymbolEqualityComparer.Default.Equals(left, GetSymbol(right));

    public void ConvertAll(Compilation compilation, bool enforce = false) {
        _cache = _keys.ToDictionary(key => key, compilation.GetTypeByMetadataName);

        if (!enforce) return;
        List<string> list = _cache.Where(kvp => kvp.Value is null)
            .Select(kvp => kvp.Key)
            .ToList();

        list.ForEach(key => throw new Exception($"Could not find {key}"));
    }
    

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerator GetEnumerator() => _keys.GetEnumerator();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();
    public int Count => _keys.Count;
    public bool IsSynchronized => false;
    public object SyncRoot => throw new NotImplementedException();
}
