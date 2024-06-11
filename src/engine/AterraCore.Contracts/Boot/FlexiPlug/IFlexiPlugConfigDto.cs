// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IFlexiPlugConfigDto {
    public bool IncludeRootAssembly { get;}
    public IEnumerable<string> PluginFilePaths { get; }
    
    public string? RootAssemblyName {get;}
    public string? RootAssemblyAuthor {get;}
    
}
