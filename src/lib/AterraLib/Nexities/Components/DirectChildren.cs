// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Extensions;

namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IDirectChildren>(AssetIdLib.AterraCore.Components.DirectChildren)]
[UsedImplicitly]
public class DirectChildren : NexitiesComponent, IDirectChildren {
    private readonly List<Ulid> _directChildren = new(12);
    private readonly ReaderWriterLockSlim _rwLock = new();
    private int? _count;
    public int Count {
        get {
            using (_rwLock.Read()) return _count ??= _directChildren.Count;
        }
    }

    public IReadOnlyCollection<Ulid> Children {
        get {
            using (_rwLock.Read()) return _directChildren.AsReadOnly();
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAddFirst(Ulid id) {
        using (_rwLock.Write()) {
            if (_directChildren.Contains(id)) return false;
            _directChildren.Insert(0, id);
            _count = null;
            return true;
        }
    }
    public bool TryAdd(Ulid id) {
        using (_rwLock.Write()) {
            if (_directChildren.Contains(id)) return false;
            _directChildren.Add(id);
            _count = null;
            return true;
        }
    }

    public bool TryInsertBefore(Ulid id, Ulid before) {
        using (_rwLock.Write()) {
            if (_directChildren.Contains(id) || !_directChildren.Contains(before)) return false;
            int indexBefore = _directChildren.IndexOf(before);
            _directChildren.Insert(
                indexBefore == 0 ? indexBefore : indexBefore - 1,
                id
            );
            _count = null;
            return true;
        }
    }
    
    public bool TryInsertAfter(Ulid id, Ulid after) {
        using (_rwLock.Write()) {
            if (_directChildren.Contains(id) || !_directChildren.Contains(after)) return false;
            int indexAfter = _directChildren.IndexOf(after);

            _directChildren.Insert(
                indexAfter == _directChildren.Count ? indexAfter : indexAfter + 1,
                id
            );
            
            _count = null;
            return true;
        }
    }
}
