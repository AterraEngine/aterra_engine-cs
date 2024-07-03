// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Lib.Components.HUD;
using AterraCore.Nexities.Lib.Components.HUD.Text;
using AterraCore.Nexities.Lib.Entities.HUD;
using AterraCore.Nexities.Systems;
using CodeOfChaos.Extensions;
using Raylib_cs;

namespace AterraCore.Nexities.Lib.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("Nexities:Systems/RenderHud", CoreTags.RenderSystem)]
public class RenderHud : NexitiesSystem<IHud> {
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
