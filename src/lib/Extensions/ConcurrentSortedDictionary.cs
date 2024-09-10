// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ConcurrentSortedDictionary<TKey, TValue> where TKey : notnull {
    private readonly ConcurrentDictionary<TKey, TValue> _dictionary = new();
    private bool _hasChanged;

    private TValue[] _preAllocatedCacheArrayValues = ArrayPool<TValue>.Shared.Rent(0);
    private ImmutableSortedSet<TKey> _sortedKeys = ImmutableSortedSet<TKey>.Empty;

    public TValue this[TKey key] {
        get => _dictionary[key];
        set {
            _dictionary[key] = value;
            UpdateKeys(set => set.Add(key));
        }
    }
    public TValue[] Values {
        get {
            if (!_hasChanged) return _preAllocatedCacheArrayValues;

            ArrayPool<TValue>.Shared.Return(_preAllocatedCacheArrayValues, true);
            _preAllocatedCacheArrayValues = ArrayPool<TValue>.Shared.Rent(_sortedKeys.Count);

            for (int i = 0; i < _sortedKeys.Count; i++)
                if (_dictionary.TryGetValue(_sortedKeys[i], out TValue? value))
                    _preAllocatedCacheArrayValues[i] = value;

            _hasChanged = false;
            return _preAllocatedCacheArrayValues;
        }
    }

    public bool TryAdd(TKey key, TValue value) {
        if (!_dictionary.TryAdd(key, value)) return false;
        UpdateKeys(set => set.Add(key));
        return true;
    }

    public bool TryRemove(TKey key) {
        if (!_dictionary.TryRemove(key, out _)) return false;
        UpdateKeys(set => set.Remove(key));
        return true;
    }

    public void Clear() {
        _dictionary.Clear();
        _sortedKeys = ImmutableSortedSet<TKey>.Empty;
        _hasChanged = true;
    }

    private void UpdateKeys(Func<ImmutableSortedSet<TKey>, ImmutableSortedSet<TKey>> updateFunc) {
        ImmutableInterlocked.Update(ref _sortedKeys, updateFunc);
        _hasChanged = true;
    }

    // private TKey[] _preAllocatedCacheArrayKeys = ArrayPool<TKey>.Shared.Rent(0);
    // public TKey[] Keys {
    //     get {
    //         if (!_hasChanged) return _preAllocatedCacheArrayKeys;
    //         
    //         ArrayPool<TKey>.Shared.Return(_preAllocatedCacheArrayKeys, clearArray: true);
    //         _preAllocatedCacheArrayKeys = ArrayPool<TKey>.Shared.Rent(_sortedKeys.Count);
    //         
    //         for (int i = 0; i < _sortedKeys.Count; i++) {
    //             TKey key = _sortedKeys[i];
    //             _preAllocatedCacheArrayKeys[i] = key;
    //         }
    //         
    //         _hasChanged = false;
    //         return _preAllocatedCacheArrayKeys;
    //     }
    // }
}
