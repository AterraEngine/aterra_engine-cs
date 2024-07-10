// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using System.Reflection;

namespace AterraCore.Contracts.Boot.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IFilePathLoadedPluginDto : IPluginDto{
    string FilePath { get; }
    string CheckSum { get; }
    IEnumerable<string> InternalFilePaths { get; set; }
    PluginConfigXml ConfigXml { get; set; }
    
    List<Assembly> Assemblies { get;}
}
