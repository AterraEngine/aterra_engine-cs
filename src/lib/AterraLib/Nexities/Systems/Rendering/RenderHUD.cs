// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraLib.Nexities.Components;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/RenderHud", CoreTags.RenderSystem)]
public class RenderHud : NexitiesSystem<IHud> {
    protected override IEnumerable<IHud> SelectEntities(INexitiesLevel? level) {
        return level.AssetTree.OfTypeManyReverse<IHud>();
    }
    protected override void ProcessEntity(IHud entity) {
        entity.AssetTree
            .OfTypeManyReverse<IHudElement>()
            .IterateOver(element => {
                switch (element.HudComponent.Type) {
                    case HudType.Text when element.HudComponent is RaylibHudText text: {
                        Raylib.DrawText(text.Text, text.Pos[0], text.Pos[1], text.FontSize, text.Color);
                        break;
                    }

                    case HudType.TextPro: {
                        break;
                    }

                    case HudType.Texture: {
                        break;
                    }

                    case HudType.Empty:
                    default: {
                        break;
                    }
                }
            });
    }
}
