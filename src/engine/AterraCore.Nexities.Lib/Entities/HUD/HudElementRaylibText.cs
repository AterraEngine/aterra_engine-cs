// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Entities;
using AterraCore.Nexities.Lib.Components.HUD;
using AterraCore.Nexities.Lib.Components.HUD.Text;

namespace AterraCore.Nexities.Lib.Entities.HUD;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class HudElementRaylibText(IAssetTree childEntities, IRaylibHudText hudComponent) : NexitiesEntity(childEntities, hudComponent), IHudElement {
    public IRaylibHudText TextComponent => hudComponent;
    public IAssetTree ChildEntities => childEntities;
    public IHudComponent HudComponent => (IHudComponent)TextComponent;
}
