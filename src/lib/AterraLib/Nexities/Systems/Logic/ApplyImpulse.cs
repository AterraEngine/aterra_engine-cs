// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/ApplyImpulse", CoreTags.LogicSystem)]
[Injectable<ApplyImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class ApplyImpulse: NexitiesSystem<IActor2D> {
    
    protected override IEnumerable<IActor2D> SelectEntities(INexitiesLevel? level) {
        return level?.AssetTree.OfType<IActor2D>().Where(actor2D => !actor2D.Impulse2D.IsEmpty) ?? [];
    }
    
    protected override void ProcessEntity(IActor2D entity) {
        entity.Transform2D.Translation += entity.Impulse2D.TranslationOffset;
        entity.Transform2D.Scale += entity.Impulse2D.ScaleOffset;
        entity.Transform2D.Rotation += entity.Impulse2D.RotationOffset;

        entity.Impulse2D.Clear();
    }
}
