// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;

namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IDirectChildren>("AterraLib:Nexities/Components/AssetTree")]
[UsedImplicitly]
public class DirectChildren : NexitiesComponent, IDirectChildren {
    private readonly List<Ulid> _directChildren = [];
    public int Count {
        get {
            lock (_directChildren) {
                return _directChildren.Count;
            }
        }
    }

    public IReadOnlyCollection<Ulid> Children {
        get {
            IReadOnlyCollection<Ulid> children;
            lock (_directChildren) {
                children = _directChildren.AsReadOnly();
            }
            return children;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAdd(Ulid id) {
        lock (_directChildren) {
            if (_directChildren.Contains(id)) return false;
            _directChildren.Add(id);
        }
        return true;
    }
    
    public bool TryInsertBefore(Ulid id, Ulid before) {
        lock (_directChildren) {
            if (_directChildren.Contains(id) || !_directChildren.Contains(before)) return false;
            int indexBefore = _directChildren.IndexOf(before);
            _directChildren.Insert(
                indexBefore == 0 ? indexBefore : indexBefore - 1,
                id
            );
        }
        return true;
    }
    
    public bool TryInsertAfter(Ulid id, Ulid after) {
        lock (_directChildren) {
            if (_directChildren.Contains(id) || !_directChildren.Contains(after)) return false;
            int indexAfter = _directChildren.IndexOf(after);

            _directChildren.Insert(
                indexAfter == _directChildren.Count ? indexAfter : indexAfter + 1,
                id
            );
        }
        return true;
    }
}
