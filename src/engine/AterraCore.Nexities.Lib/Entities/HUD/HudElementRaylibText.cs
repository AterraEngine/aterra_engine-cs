// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Lib.Entities.HUD;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using Components.HUD;
using Components.HUD.Text;
using Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class HudElementRaylibText(IAssetTree childEntities, IRaylibHudText hudComponent) : NexitiesEntity(childEntities, hudComponent), IHudElement {
    public IRaylibHudText TextComponent => hudComponent;
    public IAssetTree ChildEntities => childEntities;
    public IHudComponent HudComponent => (IHudComponent)TextComponent;
}