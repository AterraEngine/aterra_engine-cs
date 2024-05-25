// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Nexities.Lib.Components.HUD;
using Nexities.Lib.Components.HUD.Text;
namespace Nexities.Lib.Entities.HUD;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class HudElementRaylibText(IAssetTree childEntities, IRaylibHudText hudComponent) : NexitiesEntity(childEntities, hudComponent), IHudElement {
    public IRaylibHudText TextComponent => hudComponent;
    public IAssetTree ChildEntities => childEntities;
    public IHudComponent HudComponent => (IHudComponent)TextComponent;
}