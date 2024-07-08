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
public interface IPreLoadedPluginDto {
    string FilePath { get; }
    string CheckSum { get; }
    PluginValidity Validity { get; set; }
    IEnumerable<string> InternalFilePaths { get; set; }
    PluginConfigXml ConfigXml { get; set; }
    
    List<Assembly> Assemblies { get;}
    IEnumerable<Type> Types { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetInvalid();
    IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute;
}
