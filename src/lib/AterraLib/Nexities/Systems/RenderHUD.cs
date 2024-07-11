// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraLib.Nexities.Components;

namespace AterraLib.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/RenderHud", CoreTags.RenderSystem)]
public class RenderHud : NexitiesSystem<IHud> {
    protected override IEnumerable<IHud> EntitySelector(INexitiesLevel level) {
        return level.ChildEntities.OfTypeManyReverse<IHud>();
    }
    protected override void ProcessEntity(IHud entity) {
        entity.ChildEntities
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
