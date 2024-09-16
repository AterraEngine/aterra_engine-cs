// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>(StringAssetIdLib.AterraLib.Entities.Hud)]
public class Hud(IDirectChildren children) : NexitiesEntity(children), IHud {
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {
        _children = null;
    }
}
