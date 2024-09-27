// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using Extensions;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IDirectChildren>(StringAssetIdLib.AterraLib.Components.DirectChildren)]
[UsedImplicitly]
public class DirectChildren : NexitiesComponent, IDirectChildren {
    private readonly ReaderWriterLockSlim _rwLock = new();
    private int? _count;
    protected virtual List<Ulid> DirectChildIds { get; } = new(12);

    public int Count {
        get {
            using (_rwLock.Read()) return _count ??= DirectChildIds.Count;
        }
    }

    public IReadOnlyCollection<Ulid> Children {
        get {
            using (_rwLock.Read()) {
                return DirectChildIds.AsReadOnly();
            }
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAddFirst(Ulid id) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id)) return false;

            DirectChildIds.Insert(0, id);
            _count = null;
            return true;
        }
    }
    public bool TryAdd(Ulid id) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id)) return false;

            DirectChildIds.Add(id);
            _count = null;
            return true;
        }
    }

    public bool TryInsertBefore(Ulid id, Ulid before) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id) || !DirectChildIds.Contains(before)) return false;

            int indexBefore = DirectChildIds.IndexOf(before);
            DirectChildIds.Insert(
                indexBefore == 0 ? indexBefore : indexBefore - 1,
                id
            );

            _count = null;
            return true;
        }
    }

    public bool TryInsertAfter(Ulid id, Ulid after) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id) || !DirectChildIds.Contains(after)) return false;

            int indexAfter = DirectChildIds.IndexOf(after);

            DirectChildIds.Insert(
                indexAfter == DirectChildIds.Count ? indexAfter : indexAfter + 1,
                id
            );

            _count = null;
            return true;
        }
    }
}
