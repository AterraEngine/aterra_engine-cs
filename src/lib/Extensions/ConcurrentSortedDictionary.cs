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
    private ImmutableSortedSet<TKey> _sortedKeys = ImmutableSortedSet<TKey>.Empty;
    private bool _hasChanged;

    public TValue this[TKey key] {
        get => _dictionary[key];
        set {
            _dictionary[key] = value;
            UpdateKeys(set => set.Add(key));
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
    
    private TValue[] _preAllocatedCacheArray = ArrayPool<TValue>.Shared.Rent(0);
    public TValue[] Values {
        get {
            if (!_hasChanged) return _preAllocatedCacheArray;
            
            ArrayPool<TValue>.Shared.Return(_preAllocatedCacheArray, clearArray: true);
            _preAllocatedCacheArray = ArrayPool<TValue>.Shared.Rent(_sortedKeys.Count);
            
            for (int i = 0; i < _sortedKeys.Count; i++) {
                TKey key = _sortedKeys[i];
                if (_dictionary.TryGetValue(key, out TValue? value)) {
                    _preAllocatedCacheArray[i] = value;
                }
            }
            
            _hasChanged = false;
            return _preAllocatedCacheArray;
        }
    }
}