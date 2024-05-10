// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Nexities.Entities;
using Nexities.Lib.Components.HUD;
using Nexities.Lib.Components.HUD.Text;
namespace Nexities.Lib.Entities.HUD;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class HudElementRaylibText(IAssetTree childEntities, IRaylibHudText hudComponent) : NexitiesEntity(childEntities, hudComponent), IHudElement {
    public IAssetTree ChildEntities => childEntities;
    public IRaylibHudText TextComponent => hudComponent;
    public IHudComponent HudComponent => (IHudComponent)TextComponent;
}