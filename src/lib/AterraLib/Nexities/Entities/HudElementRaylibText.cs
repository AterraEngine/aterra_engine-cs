// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HudElementRaylibText(IAssetTree childEntities, IRaylibHudText hudComponent) :
    NexitiesEntity(childEntities, hudComponent), 
    IHudElement 
{
    public IRaylibHudText TextComponent => hudComponent;
    public IAssetTree ChildEntities => childEntities;
    public IHudComponent HudComponent => (IHudComponent)TextComponent;
}
