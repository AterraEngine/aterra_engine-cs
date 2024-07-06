// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraEngine.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum TextureQueueRecordType {
    Register,
    Unregister
}

public record TextureQueueRecord(
    AssetId TextureAssetId,
    string TexturePath,
    TextureQueueRecordType RecordType,
    Guid? PredefinedGuid = null
);