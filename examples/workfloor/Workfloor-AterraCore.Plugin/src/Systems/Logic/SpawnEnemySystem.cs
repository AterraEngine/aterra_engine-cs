// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.DI;
using AterraCore.Nexities.Systems;
using JetBrains.Annotations;
using System.Diagnostics;
using System.Numerics;
using Workfloor_AterraCore.Plugin.Entities;

namespace Workfloor_AterraCore.Plugin.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[System(WorkfloorIdLib.SystemsLogic.SpawnEnemy)]
public class SpawnEnemySystem(IAssetInstanceAtlas instanceAtlas, ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem, ILogicSytem {

    private EntityCollection? _enemyCollection;
    private readonly Stopwatch _stopWatch = Stopwatch.StartNew();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        if (_enemyCollection is null) {
            if (!level.ActiveEntityTree.TryGetFirst(node => node.Value is EntityCollection {TagComponent.Tags: {} tags } && tags.Contains(WorkfloorIdLib.Tags.EnemyCollection), out EntityCollection? enemyCollection)) throw new Exception("apjzepokazek");
            _enemyCollection = enemyCollection;
        }
        
        if (_stopWatch.ElapsedMilliseconds < 1000) return;
        if (_enemyCollection.Count >= 100) return;

        if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.ActorDuckyHype, out IActor2D? enemyDucky))
            throw new ApplicationException("Entity could not be created");

        enemyDucky.Transform2D.Translation = new Vector2(Random.Shared.Next(-10, 10), Random.Shared.Next(-10, 10));
        enemyDucky.Transform2D.Scale = Vector2.One;
        _enemyCollection.ChildrenIDs.TryAdd(enemyDucky);

        level.ResetActiveEntityTree();
        crossThreadDataAtlas.LevelChangeBus.NotifyLevelChange();
        
        _stopWatch.Restart();
    }

}
