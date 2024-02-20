// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.Assets;
using Serilog;

namespace AterraEngine.Core.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Asset<TDto>(ILogger logger) : IAsset where TDto : class {
    public Guid Guid { get; } = Guid.NewGuid();
    
    public bool TryPopulateFromDto(object assetDto) {
        if (assetDto is not TDto assetDto2) {
            logger.Warning(
                "asset {Guid} : AssetDTO of {Type1} could not be cast to {Type2}",
                Guid, 
                assetDto,
                typeof(TDto)
            );
            return false;
        }
        PopulateFromDto(assetDto2);
        return true;
    }
    
    public abstract void PopulateFromDto(TDto assetDto);
}