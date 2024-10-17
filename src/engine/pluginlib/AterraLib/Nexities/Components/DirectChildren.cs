// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using Extensions;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IDirectChildren>(StringAssetIdLib.AterraLib.Components.DirectChildren)]
[UsedImplicitly]
public class DirectChildren : NexitiesComponent, IDirectChildren {
    private readonly ReaderWriterLockSlim _rwLock = new();

    protected virtual List<Ulid> DirectChildIds { get; } = new(12);

    private int? _count;
    public int Count {
        get {
            using (_rwLock.Read()) {
                return _count ??= DirectChildIds.Count;
            }
        }
    }

    private int? _countNested;
    public int CountNested {
        get {
            using (_rwLock.Read()) {
                // if (_countNested is not null) return _countNested.Value;

                IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();

                Stack<Ulid> stack = new(DirectChildIds);
                int count = 0;
                while (stack.TryPop(out Ulid id)) {
                    count++;
                    if (!instanceAtlas.TryGet(id, out IHasDirectChildren? nestedChildren)) continue;

                    foreach (Ulid nestedChildId in nestedChildren.ChildrenIDs.Children) {
                        stack.Push(nestedChildId);
                    }
                }

                // _countNested = count;
                return count;
            }
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
    private void ClearCaches() {
        using (_rwLock.Write()) {
            _count = null;
            _countNested = null;
        }

    }

    public bool TryAddFirst(Ulid id) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id)) return false;

            DirectChildIds.Insert(0, id);
        }

        ClearCaches();
        return true;
    }
    public bool TryAdd(Ulid id) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id)) return false;

            DirectChildIds.Add(id);
        }

        ClearCaches();
        return true;
    }
    public bool TryAdd<T>(T asset) where T : IAssetInstance => TryAdd(asset.InstanceId);

    public bool TryInsertBefore(Ulid id, Ulid before) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id) || !DirectChildIds.Contains(before)) return false;

            int indexBefore = DirectChildIds.IndexOf(before);
            DirectChildIds.Insert(
                indexBefore == 0 ? indexBefore : indexBefore - 1,
                id
            );
        }

        ClearCaches();
        return true;
    }

    public bool TryInsertAfter(Ulid id, Ulid after) {
        using (_rwLock.Write()) {
            if (DirectChildIds.Contains(id) || !DirectChildIds.Contains(after)) return false;

            int indexAfter = DirectChildIds.IndexOf(after);

            DirectChildIds.Insert(
                indexAfter == DirectChildIds.Count ? indexAfter : indexAfter + 1,
                id
            );
        }

        ClearCaches();
        return true;
    }
}
