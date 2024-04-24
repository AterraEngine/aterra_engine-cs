// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts.Config.PluginConfig;

namespace AterraCore.Contracts.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public struct ServiceData(Type instanceType, Type serviceType) {
    public Type InstanceType { get; } = instanceType;
    public Type ServiceType { get; } = serviceType;
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginData {
    public PluginId Id { get; }
    public string FilePath { get; }
    public string ReadableName { get; }
    public bool IsProcessed { get; set; }
    
    public IPluginConfigDto<ISemanticVersion>? Data { get; set; }
    
    public PluginValidity Validity { get; set; }
    public List<Assembly> Assemblies { get; }
    public IEnumerable<Type> Types { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<Type> GetAssetTypes();
    public IEnumerable<ServiceData> GetServices();
    public void IngestFromPluginConfigDto(IPluginConfigDto<ISemanticVersion> pluginConfigDto);
}