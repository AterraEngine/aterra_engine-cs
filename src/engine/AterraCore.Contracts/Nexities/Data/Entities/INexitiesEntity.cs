// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Components;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesEntity : IAssetInstance {
    public ConcurrentDictionary<AssetId, IComponent> Components { get; set; }
    public IEnumerable<IComponent> ComponentsArray { get; }
}
