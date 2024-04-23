// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.FlexiPlug.Attributes;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginData(int id, string filepath) : IPluginData {
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string[] Dlls { get; set; }

    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ?  value : _validity ; // Once invalid, always invalid
    }

    private Assembly? _assembly;
    public Assembly? Assembly {
        get => _assembly;
        set {
            if (_assembly == null) return;
            
            _assembly = value;
            _types = _assembly!.GetTypes();
        }
    }

    private IEnumerable<Type>? _types ;
   

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<Type> GetAssetTypes() {
        return _types!
            .Where(t => 
                typeof(IAsset).IsAssignableFrom(t) 
                && t is { IsInterface: false, IsAbstract: false }
        );
    }

    public IEnumerable<ServiceData> GetServices() {
        return _types!
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ServiceAttribute>() }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceData(t.Attribute?.Interface!, t.Type));
    }
    
}