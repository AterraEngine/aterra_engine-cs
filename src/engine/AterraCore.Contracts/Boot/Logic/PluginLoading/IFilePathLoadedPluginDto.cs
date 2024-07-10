// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using System.Reflection;

namespace AterraCore.Contracts.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IFilePathLoadedPluginDto : IPluginDto {
    string FilePath { get; }
    string CheckSum { get; }
    IEnumerable<string> InternalFilePaths { set; }
    PluginConfigXml ConfigXml { get; set; }
    
    List<Assembly> Assemblies { get;}
}
