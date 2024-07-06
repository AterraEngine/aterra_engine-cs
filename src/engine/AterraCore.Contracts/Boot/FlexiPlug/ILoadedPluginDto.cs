// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Contracts.Boot.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoadedPluginDto {
    public string NameSpace{ get; }
    public string NameReadable { get; }
    public List<Assembly> Assemblies { get; }
    public bool IsProcessed { get; set; }
    public PluginValidity Validity { get; set; }
    public string? CheckSum { get; set; }
    public IEnumerable<Type> Types { get; }
    public string FilePath { get; }
    
    public PluginConfigXml? Data { get; set; }
    public IEnumerable<string> InternalFilePaths { get; set; }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServicesDefault();
    public IEnumerable<ServiceDescriptor> GetServicesStatic();
}
